using System;
using System.Linq;
using brightcast.Entities;
using brightcast.Helpers;

namespace brightcast.Services
{
    public interface IRoleService
    {
        Role GetById(int id);
        Role GetByUserProfileId(int userProfileId);
        Role Create(Role role);
        void Update(Role role);
        void Delete(int id);

    }

    public class RoleService : IRoleService
    {
        private DataContext _context;

        public RoleService(DataContext context)
        {
            _context = context;
        }

        public Role GetById(int id)
        {
            var role = _context.Roles.Find(id);

            return role != null && role.Deleted == 0 ? role : null;
        }

        public Role GetByUserProfileId(int userProfileId)
        {
            return _context.UserProfiles.Find(userProfileId).Role;
        }


        public Role Create(Role role)
        {
            // validation


            role.CreatedAt = DateTime.UtcNow;
            role.CreatedBy = "API";

            role.Deleted = 0;

            _context.Roles.Add(role);
            _context.SaveChanges();

            return role;
        }

        public void Update(Role roleParam)
        {
            var role = _context.Roles.Find(roleParam.Id);

            if (role == null || role.Deleted == 1)
                throw new AppException("Role not found");

            // update Name if it has changed
            if (!string.IsNullOrWhiteSpace(roleParam.Name) && roleParam.Name != role.Name)
            {
                role.Name = roleParam.Name;
            }

            // update lastName if it has changed
            if (roleParam.Scope != null && !roleParam.Scope.SequenceEqual(role.Scope))
            {
                role.Scope = roleParam.Scope;
            }

            // update user properties if provided

            role.UpdatedBy = roleParam.UpdatedBy;

            role.UpdatedAt = roleParam.UpdatedAt;
            
            _context.Roles.Update(role);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var role = _context.Roles.Find(id);
            if (role != null)
            {
                role.Deleted = 1;
                
                _context.Roles.Update(role);
                _context.SaveChanges();
            }
        }

    }
}