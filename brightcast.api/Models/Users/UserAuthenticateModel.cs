using System.ComponentModel.DataAnnotations;

namespace brightcast.Models.Users
{
    public class UserAuthenticateModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}