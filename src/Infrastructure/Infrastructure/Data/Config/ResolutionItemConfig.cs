using Domain.Entities.ResolutionManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    /// <summary>
    /// Entity Framework configuration for ResolutionItem entity
    /// Configures table structure, relationships, and constraints
    /// </summary>
    public class ResolutionItemConfig : IEntityTypeConfiguration<ResolutionItem>
    {
        public void Configure(EntityTypeBuilder<ResolutionItem> builder)
        {
            // Table configuration
            builder.ToTable("ResolutionItems");
            
            // Primary key
            builder.HasKey(x => x.Id);
            
            // Properties configuration
            builder.Property(x => x.ResolutionId)
                .IsRequired()
                .HasComment("Foreign key reference to Resolution entity");
                
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("Auto-generated title (Item1, Item2, etc.)");
                
            builder.Property(x => x.Description)
                .HasMaxLength(500)
                .HasComment("Description of the resolution item (max 500 characters)");
                
            builder.Property(x => x.HasConflict)
                .IsRequired()
                .HasDefaultValue(false)
                .HasComment("Indicates if there is a conflict of interest with board members");
                
            builder.Property(x => x.DisplayOrder)
                .IsRequired()
                .HasComment("Display order for sorting resolution items");
            
            // Relationships configuration
            builder.HasOne(x => x.Resolution)
                .WithMany(x => x.ResolutionItems)
                .HasForeignKey(x => x.ResolutionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ResolutionItems_Resolutions");
            
            // Indexes for performance
            builder.HasIndex(x => x.ResolutionId)
                .HasDatabaseName("IX_ResolutionItems_ResolutionId");
                
            builder.HasIndex(x => new { x.ResolutionId, x.DisplayOrder })
                .HasDatabaseName("IX_ResolutionItems_Resolution_Order");
                
            builder.HasIndex(x => new { x.ResolutionId, x.HasConflict })
                .HasDatabaseName("IX_ResolutionItems_Resolution_Conflict");
            
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
