using AutoMapper;
using brightcast.Entities;
using brightcast.Models.Businesses;
using brightcast.Models.Campaigns;
using brightcast.Models.ContactLists;
using brightcast.Models.Contacts;
using brightcast.Models.Roles;
using brightcast.Models.Users;

namespace brightcast.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserModel, User>();
            CreateMap<RegisterModel, User>();
            CreateMap<UpdateModel, User>();
            CreateMap<RegisterProfileModel, User>();
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