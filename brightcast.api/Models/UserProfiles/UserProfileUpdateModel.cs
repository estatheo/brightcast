namespace brightcast.Models.UserProfiles
{
  public class UserProfileUpdateModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PictureUrl { get; set; }
        public string Role { get; set; }
        public string BusinessRole { get; set; }
        public string Phone { get; set; }
        public bool Default { get; set; }
        public int BusinessId { get; set; }
        public int RoleId { get; set; }
    }
}