using System;
using System.Collections.Generic;
using System.Linq;
using brightcast.Entities;
using brightcast.Helpers;

namespace brightcast.Services
{
    public interface IContactListService
    {
        ContactList GetById(int id);
        List<ContactList> GetAllByUserProfileId(int userProfileId);
        ContactList Create(ContactList contactList);
        void Update(ContactList contactList);
        void Delete(int id);

    }

    public class ContactListService : IContactListService
    {
        private DataContext _context;

        public ContactListService(DataContext context)
        {
            _context = context;
        }

        public ContactList GetById(int id)
        {
            var contactList = _context.ContactLists.Find(id);

            return contactList != null && contactList.Deleted == 0 ? contactList : null;
        }

        public List<ContactList> GetAllByUserProfileId(int userProfileId)
        {

            return _context.ContactLists.Where(x => x.UserProfileId == userProfileId && x.Deleted == 0).ToList();
        }


        public ContactList Create(ContactList contactList)
        {
            // validation


            contactList.CreatedAt = DateTime.UtcNow;
            contactList.CreatedBy = "API";

            contactList.Deleted = 0;

            _context.ContactLists.Add(contactList);
            _context.SaveChanges();

            return contactList;
        }

        public void Update(ContactList contactParam)
        {
            var contactList = _context.ContactLists.Find(contactParam.Id);

            if (contactList == null || contactList.Deleted == 1)
                throw new AppException("ContactList not found");

            // update Name if it has changed
            if (!string.IsNullOrWhiteSpace(contactParam.Name) && contactParam.Name != contactList.Name)
            {
                contactList.Name = contactParam.Name;
            }
            
            if (!string.IsNullOrWhiteSpace(contactParam.FileUrl) && contactParam.FileUrl != contactList.FileUrl)
            {
                contactList.FileUrl = contactParam.FileUrl;
            }

            // update user properties if provided

            contactList.UpdatedBy = contactParam.UpdatedBy;

            contactList.UpdatedAt = contactParam.UpdatedAt;
            
            _context.ContactLists.Update(contactList);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var contactList = _context.ContactLists.Find(id);
            if (contactList != null)
            {
                contactList.Deleted = 1;
                
                _context.ContactLists.Update(contactList);
                _context.SaveChanges();
            }
        }

    }
}