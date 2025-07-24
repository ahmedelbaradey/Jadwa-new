using Domain.Entities.FundManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    /// <summary>
    /// Entity Framework configuration for BoardMember entity
    /// Configures table structure, relationships, and constraints
    /// </summary>
    public class BoardMemberConfig : IEntityTypeConfiguration<BoardMember>
    {
        public void Configure(EntityTypeBuilder<BoardMember> builder)
        {
            // Table configuration
            builder.ToTable("BoardMembers");
            
            // Primary key
            builder.HasKey(x => x.Id);
            
            // Properties configuration
            builder.Property(x => x.FundId)
                .IsRequired()
                .HasComment("Foreign key reference to Fund entity");
                
            builder.Property(x => x.UserId)
                .IsRequired()
                .HasComment("Foreign key reference to User entity");
                
            builder.Property(x => x.MemberType)
                .IsRequired()
                .HasConversion<int>()
                .HasComment("Type of board member (Independent=1, NotIndependent=2)");
                
            builder.Property(x => x.IsChairman)
                .IsRequired()
                .HasDefaultValue(false)
                .HasComment("Indicates if this board member is the chairman");
                
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .HasComment("Status of the board member (Active/Inactive)");
            
            // Relationships configuration
            builder.HasOne(x => x.Fund)
                .WithMany(x => x.BoardMembers)
                .HasForeignKey(x => x.FundId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_BoardMembers_Funds");
                
            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_BoardMembers_Users");
            
            // Indexes for performance
            builder.HasIndex(x => x.FundId)
                .HasDatabaseName("IX_BoardMembers_FundId");
                
            builder.HasIndex(x => x.UserId)
                .HasDatabaseName("IX_BoardMembers_UserId");
                
            builder.HasIndex(x => new { x.FundId, x.UserId })
                .IsUnique()
                .HasDatabaseName("IX_BoardMembers_Fund_User_Unique")
                .HasFilter("[IsDeleted] = 0");
                
            builder.HasIndex(x => new { x.FundId, x.IsChairman })
                .HasDatabaseName("IX_BoardMembers_Fund_Chairman")
                .HasFilter("[IsChairman] = 1 AND [IsDeleted] = 0");
                
            builder.HasIndex(x => new { x.FundId, x.MemberType, x.IsActive })
                .HasDatabaseName("IX_BoardMembers_Fund_Type_Active")
                .HasFilter("[IsDeleted] = 0");
            
            // Audit fields configuration (inherited from FullAuditedEntity)
            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");
                
            builder.Property(x => x.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
                
            builder.Property(x => x.IsDeleted)
                .HasDefaultValue(false);
                
            builder.Property(x => x.DeletedAt)
                .IsRequired(false);
            
            // Soft delete filter
            builder.HasQueryFilter(x => !x.IsDeleted.HasValue || !x.IsDeleted.Value);
        }
    }
}
