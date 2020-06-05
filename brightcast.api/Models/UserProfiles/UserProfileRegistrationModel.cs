using System.ComponentModel.DataAnnotations;

namespace brightcast.Models.UserProfiles
{
    public class UserProfileRegistrationModel
    {
        public int UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public bool Default { get; set; }
        public int BusinessId { get; set; }
        public int RoleId { get; set; }
    }
}