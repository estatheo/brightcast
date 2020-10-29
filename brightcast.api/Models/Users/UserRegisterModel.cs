using System.ComponentModel.DataAnnotations;

namespace brightcast.Models.Users
{
    public class UserRegisterModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
        public string FullName { get; set; }
        public string BusinessName { get; set; }
    }
}