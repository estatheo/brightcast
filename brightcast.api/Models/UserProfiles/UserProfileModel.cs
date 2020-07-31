using System.Collections.Generic;

namespace brightcast.Models.UserProfiles
{
  public class UserProfileModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PictureUrl { get; set; }
        public string Phone { get; set; }
        public string BusinessRole { get; set; }
        public string Role { get; set; }
        public List<string> Scope { get; set; }

    }
}