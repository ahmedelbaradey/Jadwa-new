using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Domain.Entities.Users;
using Domain.Entities.Base;
using Infrastructure.Data.Configurations;


namespace Infrastructure.Data
{
    public abstract class AuditableDbContext : IdentityDbContext<User, Role,
        int, IdentityUserClaim<int>, IdentityUserRole<int>, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public AuditableDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        #region Functions
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
               => optionsBuilder
               .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            modelBuilder.ConfigureIndexes();
            base.OnModelCreating(modelBuilder);
        }
        public virtual async Task<int> SaveChangesAsync(int userId = 1)
        {
             
            foreach (var entity in ChangeTracker.Entries<CreationAuditedEntity>())
            {
                if (entity.State == EntityState.Added)
                {
                    entity.Entity.CreatedAt = DateTime.Now;
                    entity.Entity.CreatedBy = userId;
                }
            }

            foreach (var entity in ChangeTracker.Entries<AduitedEntity>())
            {
                if (entity.State == EntityState.Modified)
                {
                    entity.Entity.UpdatedAt = DateTime.Now;
                    entity.Entity.UpdatedBy = userId;
                }
                if(entity.State == EntityState.Added)
                {
                    entity.Entity.UpdatedAt = DateTime.Now;
                }
            }

            foreach (var entity in ChangeTracker.Entries<FullAuditedEntity>())
            {
                if (entity.State == EntityState.Modified && entity.Entity.IsDeleted.HasValue && entity.Entity.IsDeleted.Value)
                {
                    entity.Entity.DeletedAt = DateTime.Now;
                    entity.Entity.DeletedBy = userId;
                }
            }

            var result = await base.SaveChangesAsync();
            return result;
        }
        #endregion

    }
}
