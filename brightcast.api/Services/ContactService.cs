using System;
using System.Collections.Generic;
using brightcast.Entities;
using brightcast.Helpers;

namespace brightcast.Services
{
    public interface IContactService
    {
        Contact GetById(int id);
        List<Contact> GetAllByContactListId(int contactListId);
        Contact Create(Contact contact);
        void Update(Contact contact);
        void Delete(int id);

    }

    public class ContactService : IContactService
    {
        private DataContext _context;

        public ContactService(DataContext context)
        {
            _context = context;
        }

        public Contact GetById(int id)
        {
            var contact = _context.Contacts.Find(id);

            return contact != null && contact.Deleted == 0 ? contact : null;
        }

        public List<Contact> GetAllByContactListId(int contactListId)
        {

            return _context.ContactLists.Find(contactListId).Contacts;
        }


        public Contact Create(Contact contact)
        {
            // validation


            contact.CreatedAt = DateTime.UtcNow;
            contact.CreatedBy = "API";

            contact.Deleted = 0;

            _context.Contacts.Add(contact);
            _context.SaveChanges();

            return contact;
        }

        public void Update(Contact contactParam)
        {
            var contact = _context.Contacts.Find(contactParam.Id);

            if (contact == null || contact.Deleted == 1)
                throw new AppException("Contact not found");

            // update Name if it has changed
            if (!string.IsNullOrWhiteSpace(contactParam.FirstName) && contactParam.FirstName != contact.FirstName)
            {
                contact.FirstName = contactParam.FirstName;
            }
            
            if (!string.IsNullOrWhiteSpace(contactParam.LastName) && contactParam.LastName != contact.LastName)
            {
                contact.LastName = contactParam.LastName;
            }
            
            if (!string.IsNullOrWhiteSpace(contactParam.Phone) && contactParam.Phone != contact.Phone)
            {
                contact.Phone = contactParam.Phone;
            }

            if (!string.IsNullOrWhiteSpace(contactParam.Email) && contactParam.Email != contact.Email)
            {
                contact.Email = contactParam.Email;
            }

            // update user properties if provided

            contact.UpdatedBy = contactParam.UpdatedBy;

            contact.UpdatedAt = contactParam.UpdatedAt;
            
            _context.Contacts.Update(contact);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var contact = _context.Contacts.Find(id);
            if (contact != null)
            {
                contact.Deleted = 1;
                
                _context.Contacts.Update(contact);
                _context.SaveChanges();
            }
        }

    }
}