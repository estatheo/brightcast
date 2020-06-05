using AutoMapper;
using brightcast.Entities;
using brightcast.Models.Businesses;
using brightcast.Models.Campaigns;
using brightcast.Models.ContactLists;
using brightcast.Models.Contacts;
using brightcast.Models.Roles;
using brightcast.Models.UserProfiles;
using brightcast.Models.Users;

namespace brightcast.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserModel, User>();
            CreateMap<UserRegisterModel, User>();
            CreateMap<UserUpdateModel, User>();
            CreateMap<UserAuthenticateModel, User>();
            CreateMap<UserProfileUpdateModel, UserProfile>();
            CreateMap<UserProfileRegistrationModel, UserProfile>();
            CreateMap<UserProfileModel, UserProfile>();
            CreateMap<RoleModel, Role>();
            CreateMap<ContactModel, Contact>();
            CreateMap<ContactListModel, ContactList>();
            CreateMap<CampaignModel, Campaign>();
            CreateMap<CampaignSentModel, CampaignSent>();
            CreateMap<CampaignSentStatsModel, CampaignSentStats>();
            CreateMap<BusinessModel, Business>();
            

        }
    }
}