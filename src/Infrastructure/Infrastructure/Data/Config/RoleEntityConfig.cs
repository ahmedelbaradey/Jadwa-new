using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    /// <summary>
    /// Entity Framework configuration for Role entity
    /// Configures navigation properties and relationships for optimized user-role retrieval
    /// Provides the inverse navigation configuration required by Entity Framework
    /// </summary>
    public class RoleEntityConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            // Configure the many-to-many relationship between Role and User through IdentityUserRole
            builder
                .HasMany(r => r.Users)
                .WithMany(u => u.Roles)
                .UsingEntity<IdentityUserRole<int>>(
                    // Configure the relationship from IdentityUserRole to User
                    userRole => userRole
                        .HasOne<User>()
                        .WithMany(u => u.UserRoles)
                        .HasForeignKey(ur => ur.UserId)
                        .OnDelete(DeleteBehavior.Cascade),
                    // Configure the relationship from IdentityUserRole to Role
                    userRole => userRole
                        .HasOne<Role>()
                        .WithMany(r => r.UserRoles)
                        .HasForeignKey(ur => ur.RoleId)
                        .OnDelete(DeleteBehavior.Cascade),
                    // Configure the IdentityUserRole entity itself
                    userRole =>
                    {
                        userRole.HasKey(ur => new { ur.UserId, ur.RoleId });
                        userRole.ToTable("AspNetUserRoles");
                    });

            // Configure role properties with proper constraints
            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(256)
                .HasComment("Role name (e.g., Fund Manager, Legal Council, Board Secretary, Board Member)");

            builder.Property(r => r.NormalizedName)
                .IsRequired()
                .HasMaxLength(256)
                .HasComment("Normalized role name for case-insensitive lookups");

            builder.Property(r => r.ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasComment("Concurrency token for optimistic concurrency control");

            // Configure indexes for performance optimization
            builder.HasIndex(r => r.NormalizedName)
                .IsUnique()
                .HasDatabaseName("RoleNameIndex")
                .HasFilter("[NormalizedName] IS NOT NULL");

            // Configure table name to match ASP.NET Core Identity conventions
            builder.ToTable("AspNetRoles");
        }
    }
}
