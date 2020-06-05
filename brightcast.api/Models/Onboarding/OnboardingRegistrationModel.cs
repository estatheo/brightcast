using brightcast.Entities;
using brightcast.Models.Businesses;
using brightcast.Models.Campaigns;
using brightcast.Models.ContactLists;
using brightcast.Models.Roles;
using brightcast.Models.UserProfiles;

namespace brightcast.Models.Onboarding
{
    public class OnboardingRegistrationModel
    {
        public BusinessModel Business { get; set; }
        public RoleModel Role { get; set; }

        public UserProfileRegistrationModel UserProfile { get; set; }
        public ContactListModel ContactList { get; set; }
        public CampaignModel Campaign { get; set; }
    }
}
