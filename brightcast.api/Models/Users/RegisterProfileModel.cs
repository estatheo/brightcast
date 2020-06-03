using System.ComponentModel.DataAnnotations;

namespace brightcast.Models.Users
{
    public class RegisterProfileModel
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        public string Phone { get; set; }
    }
}