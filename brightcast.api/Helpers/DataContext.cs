using brightcast.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace brightcast.Helpers
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server database
            options.UseSqlServer(Configuration.GetConnectionString("WebApiDatabase"));
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactList> ContactLists { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<CampaignSent> CampaignSents { get; set; }
        public DbSet<CampaignSentStats> CampaignSentStatses { get; set; }
        public DbSet<Business> Businesses { get; set; }
    }
}