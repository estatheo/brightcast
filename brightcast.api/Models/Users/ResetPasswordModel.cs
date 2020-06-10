using System;

namespace brightcast.Models.Users
{
  public class ResetPasswordModel
    {
        public Guid Code { get; set; }
        public string Password { get; set; }
    }
}