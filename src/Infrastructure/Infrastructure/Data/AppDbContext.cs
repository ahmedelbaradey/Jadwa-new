using Domain.Entities.Products;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Shared;
using Domain.Entities.Startegies;
using Domain.Entities.FundManagement;
using Domain.Entities.Notifications;
using Domain.Entities.ResolutionManagement;

namespace Infrastructure.Data
{
    public class AppDbContext : AuditableDbContext 
    {
        #region Fileds

        //public DbSet<Product> Products { get; set; }
        //public DbSet<DemoEntity> DemoEntities { get; set; }
        //public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserAuditHistory> UserAuditHistories { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Strategy> Strategies { get; set; }
        public DbSet<Fund> Funds { get; set; }
        public DbSet<StatusHistory> StatusHistories { get; set; }
        public DbSet<FundStatusHistory> FundStatusHistories { get; set; }
        public DbSet<FundBoardSecretary> FundBoardSecretaries { get; set; }
        public DbSet<FundManager> FundManagers { get; set; }
        public DbSet<FundMember> FundMembers { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        // Board Member Management
        public DbSet<BoardMember> BoardMembers { get; set; }

        // Resolution Management
        public DbSet<Resolution> Resolutions { get; set; }
        public DbSet<ResolutionItem> ResolutionItems { get; set; }
        public DbSet<ResolutionItemConflict> ResolutionItemConflicts { get; set; }
        public DbSet<ResolutionType> ResolutionTypes { get; set; }
        public DbSet<ResolutionAttachment> ResolutionAttachments { get; set; }
        public DbSet<ResolutionStatusHistory> ResolutionStatusHistories { get; set; }
        public DbSet<ResolutionVote> ResolutionVotes { get; set; }

        #endregion

        #region Constructors

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        #endregion

        #region Model Configuration

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply entity configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        #endregion
    }
}
