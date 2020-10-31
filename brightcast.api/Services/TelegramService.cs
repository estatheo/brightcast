using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using brightcast.Entities;
using brightcast.Helpers;
using Microsoft.Extensions.Options;
using TeleSharp.TL;
using TLSharp.Core;
using TLUser = TeleSharp.TL.TLUser;

namespace brightcast.Services
{
    public interface ITelegramService
    {
        Task Init();
        Task MakeAuth(string code);
        Task SendCampaignMessage(string textMessage, List<Contact> contacts, UserProfile userProfile, int campaignId);
        Task SendMessage(string textMessage, Contact contact, UserProfile userProfile, int campaignId);

    }

    public class TelegramService: ITelegramService
    {
        private TelegramClient client;
        private readonly AppSettings _appSettings;
        private DataContext _context;
        private string hash;
        private TLUser telegramUser; 

        public TelegramService(IOptions<AppSettings> appSettings, DataContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;
            Init();
        }

        async Task Init()
        {
            var client = new TelegramClient(int.Parse(_appSettings.TelegramApiId), _appSettings.TelegramApiHash);
            await client.ConnectAsync();
            hash = await client.SendCodeRequestAsync(_appSettings.TelegramApiPhone);
        }

        async Task MakeAuth(string code)
        {
            telegramUser = await client.MakeAuthAsync(_appSettings.TelegramApiPhone, hash, code);
        }

        async Task SendCampaignMessage(string textMessage, List<Contact> contacts, UserProfile userProfile, int campaignId)
        {
            var contactsToImport = new List<TLInputPhoneContact>();
            foreach (var contact in contacts)
            {
                contactsToImport.Add(new TLInputPhoneContact(){Phone = contact.Phone, FirstName = contact.FirstName, LastName = contact.LastName});
            }

            var importedContacts = await client.ImportContactsAsync(contactsToImport);

            var telegramContactsResponse = await client.GetContactsAsync();

            var telegramContacts = telegramContactsResponse.Users
                .Where(x => x.GetType() == typeof(TLUser))
                .Cast<TLUser>().ToList();

            foreach (var importedContact in importedContacts.Imported)
            {
                var c = contacts.FirstOrDefault(x =>
                    x.Phone == telegramContacts.First(x => x.Id == importedContact.UserId).Phone);

                c.TelegramUserId = importedContact.UserId;
                c.UpdatedAt = DateTime.UtcNow;
                c.UpdatedBy = "TelegramService-SendCampaignMessage";
                _context.Contacts.Update(c);

                await client.SendMessageAsync(
                    new TLInputPeerUser() { UserId = importedContact.UserId }, textMessage);

                _context.ChatMessages.Add(new ChatMessage()
                {
                    Text = textMessage,
                    CreatedAt = DateTime.UtcNow,
                    Reply = true,
                    Type = "text",
                    Files = "",
                    AvatarUrl = "",
                    SenderId = userProfile.Id,
                    SenderName = userProfile.FirstName + " " + userProfile.LastName,
                    CampaignId = campaignId,
                    ContactId = c.Id,
                    Status = 2,
                    Channel = "telegram"
                });
            }

            _context.SaveChanges();
        }

        async Task SendMessage(string textMessage, Contact contact, UserProfile userProfile, int campaignId)
        {
            var importedContacts = await client.ImportContactsAsync(new List<TLInputPhoneContact>(){ new TLInputPhoneContact() { Phone = contact.Phone, FirstName = contact.FirstName, LastName = contact.LastName } });
            var importedContact = importedContacts.Imported.First();

            contact.TelegramUserId = importedContact.UserId;
            contact.UpdatedAt = DateTime.UtcNow;
            contact.UpdatedBy = "TelegramService-SendCampaignMessage";
            _context.Contacts.Update(contact);

            await client.SendMessageAsync(
                new TLInputPeerUser() { UserId = importedContact.UserId }, textMessage);

            _context.ChatMessages.Add(new ChatMessage()
            {
                Text = textMessage,
                CreatedAt = DateTime.UtcNow,
                Reply = true,
                Type = "text",
                Files = "",
                AvatarUrl = "",
                SenderId = userProfile.Id,
                SenderName = userProfile.FirstName + " " + userProfile.LastName,
                CampaignId = campaignId,
                ContactId = contact.Id,
                Status = 2,
                Channel = "telegram"
            });

            _context.SaveChanges();

        }
    }
}
