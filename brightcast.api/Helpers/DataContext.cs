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
            options.UseSqlServer(Configuration.GetConnectionString("localDB"));

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CampaignContactList>().HasKey(ccl => new {ccl.CampaignId, ccl.ContactListId});

            modelBuilder.Entity<CampaignContactList>().HasOne(ccl => ccl.Campaign).WithMany(cl => cl.CampaignContactLists).HasForeignKey(bc => bc.CampaignId);

            modelBuilder.Entity<CampaignContactList>().HasOne(ccl => ccl.ContactList).WithMany(c => c.CampaignContactLists).HasForeignKey(bc => bc.ContactListId);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserActivation> UserActivations { get; set; }
        public DbSet<ResetPassword> ResetPasswords { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactList> ContactLists { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<CampaignContactList> CampaignContactLists { get; set; }
        public DbSet<TemplateMessage> TemplateMessages { get; set; }
        public DbSet<ReceiveMessage> ReceiveMessages { get; set; }
        public DbSet<CampaignMessage> CampaignMessages { get; set; }
        public DbSet<CampaignSent> CampaignSents { get; set; }
        public DbSet<CampaignSentStats> CampaignSentStatses { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
    }
}