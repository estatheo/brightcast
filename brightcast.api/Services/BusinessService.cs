using System;
using System.Linq;
using brightcast.Entities;
using brightcast.Helpers;

namespace brightcast.Services
{
    public interface IBusinessService
    {
        Business GetById(int id);
        Business GetByUserProfileId(int userProfileId);
        Business Create(Business business);
        void Update(Business business);
        void UpdateMembership(int userProfileId, string membership);
        void Delete(int id);

    }

    public class BusinessService : IBusinessService
    {
        private DataContext _context;

        public BusinessService(DataContext context)
        {
            _context = context;
        }

        public Business GetById(int id)
        {
            var business = _context.Businesses.Find(id);

            return business != null && business.Deleted == 0 ? business : null;
        }

        public Business GetByUserProfileId(int userProfileId)
        {
            var businessId = _context.UserProfiles.Find(userProfileId).BusinessId;
            var business = _context.Businesses.Find(businessId);

            return business != null && business.Deleted == 0 ? business : null;

        }



        public Business Create(Business business)
        {
            // validation


            business.CreatedAt = DateTime.UtcNow;
            business.CreatedBy = "API";

            business.Deleted = 0;

            _context.Businesses.Add(business);
            _context.SaveChanges();

            return business;
        }

        public void Update(Business businessParam)
        {
            var business = _context.Businesses.Find(businessParam.Id);

            if (business == null || business.Deleted == 1)
                throw new AppException("Business not found");

            // update Name if it has changed
            if (!string.IsNullOrWhiteSpace(businessParam.Name) && businessParam.Name != business.Name)
            {
                business.Name = businessParam.Name;
            }

            if (!string.IsNullOrWhiteSpace(businessParam.Address) && businessParam.Address != business.Address)
            {
                business.Address = businessParam.Address;
            }

            if (!string.IsNullOrWhiteSpace(businessParam.Email) && businessParam.Email != business.Email)
            {
                business.Email = businessParam.Email;
            }

            if (!string.IsNullOrWhiteSpace(businessParam.Membership) && businessParam.Membership != business.Membership)
            {
                business.Membership = businessParam.Membership;
            }

            if (!string.IsNullOrWhiteSpace(businessParam.Website) && businessParam.Website != business.Website)
            {
                business.Website = businessParam.Website;
            }

            if (!string.IsNullOrWhiteSpace(businessParam.Category) && businessParam.Category != business.Category)
            {
                business.Category = businessParam.Category;
            }

            // update user properties if provided

            business.UpdatedBy = businessParam.UpdatedBy;

            business.UpdatedAt = businessParam.UpdatedAt;
            
            _context.Businesses.Update(business);
            _context.SaveChanges();
        }

        public void UpdateMembership(int userProfileId, string membership)
        {
            var business = _context.Businesses.SingleOrDefault(x =>
                x.Id == _context.UserProfiles.SingleOrDefault(y => y.Id == userProfileId).BusinessId);
            if (business != null)
            {
                business.Membership = membership;

                _context.Businesses.Update(business);
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var business = _context.Businesses.Find(id);
            if (business != null)
            {
                business.Deleted = 1;
                
                _context.Businesses.Update(business);
                _context.SaveChanges();
            }
        }

    }
}