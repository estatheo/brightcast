using System.Collections.Generic;

namespace brightcast.Models.Roles
{
  public class RoleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Scope { get; set; }

        public int UserProfileId { get; set; }
        public int BusinessId { get; set; }
    }
}