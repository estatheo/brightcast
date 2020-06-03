using System;
using System.Collections.Generic;
using System.Linq;
using brightcast.Entities;
using brightcast.Helpers;

namespace brightcast.Services
{
    public interface IUserProfileService
    {
        UserProfile GetById(int id);
        List<UserProfile> GetAllByUserId(int id);
        UserProfile Create(UserProfile profile);
        void Update(UserProfile profile);
        void Delete(int id);

    }

    public class UserProfileService : IUserProfileService
    {
        private DataContext _context;

        public UserProfileService(DataContext context)
        {
            _context = context;
        }

        public UserProfile GetById(int id)
        {
            var profile = _context.UserProfiles.Find(id);

            return profile != null && profile.Deleted == 0 ? profile : null;
        }

        public List<UserProfile> GetAllByUserId(int id)
        {
            return _context.UserProfiles.Where(x => x.UserId == id).Where(x => x.Deleted == 1).ToList();
        }


        public UserProfile Create(UserProfile profile)
        {
            // validation


            profile.CreatedAt = DateTime.UtcNow;
            profile.CreatedBy = "API";

            profile.Deleted = 0;

            _context.UserProfiles.Add(profile);
            _context.SaveChanges();

            return profile;
        }

        public void Update(UserProfile profileParam)
        {
            var profile = _context.UserProfiles.Find(profileParam.Id);

            if (profile == null || profile.Deleted == 1)
                throw new AppException("UserProfile not found");

            // update firstName if it has changed
            if (!string.IsNullOrWhiteSpace(profileParam.FirstName) && profileParam.FirstName != profile.FirstName)
            {
                profile.FirstName = profileParam.FirstName;
            }

            // update lastName if it has changed
            if (!string.IsNullOrWhiteSpace(profileParam.LastName) && profileParam.LastName != profile.LastName)
            {
                profile.LastName = profileParam.LastName;
            }

            // update phone if it has changed
            if (!string.IsNullOrWhiteSpace(profileParam.Phone) && profileParam.Phone != profile.Phone)
            {
                profile.Phone = profileParam.Phone;
            }

            // update pictureUrl if it has changed
            if (!string.IsNullOrWhiteSpace(profileParam.PictureUrl) && profileParam.PictureUrl != profile.PictureUrl)
            {
                profile.PictureUrl = profileParam.PictureUrl;
            }

            // update pictureUrl if it has changed
            if (!string.IsNullOrWhiteSpace(profileParam.PictureUrl) && profileParam.PictureUrl != profile.PictureUrl)
            {
                profile.PictureUrl = profileParam.PictureUrl;
            }

            // update user properties if provided

            profile.UpdatedBy = profileParam.UpdatedBy;

            profile.UpdatedAt = profileParam.UpdatedAt;
            
            _context.UserProfiles.Update(profile);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var profile = _context.UserProfiles.Find(id);
            if (profile != null)
            {
                profile.Deleted = 1;
                
                _context.UserProfiles.Update(profile);
                _context.SaveChanges();
            }
        }

    }
}