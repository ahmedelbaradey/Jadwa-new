using Domain.Entities.ResolutionManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    /// <summary>
    /// Entity Framework configuration for ResolutionItemConflict entity
    /// Configures table structure, relationships, and constraints
    /// </summary>
    public class ResolutionItemConflictConfig : IEntityTypeConfiguration<ResolutionItemConflict>
    {
        public void Configure(EntityTypeBuilder<ResolutionItemConflict> builder)
        {
            // Table configuration
            builder.ToTable("ResolutionItemConflicts");
            
            // Primary key
            builder.HasKey(x => x.Id);
            
            // Properties configuration
            builder.Property(x => x.ResolutionItemId)
                .IsRequired()
                .HasComment("Foreign key reference to ResolutionItem entity");
                
            builder.Property(x => x.BoardMemberId)
                .IsRequired()
                .HasComment("Foreign key reference to BoardMember entity");
                
            builder.Property(x => x.ConflictNotes)
                .HasMaxLength(500)
                .HasComment("Optional notes about the nature of the conflict");
            
            // Relationships configuration
            builder.HasOne(x => x.ResolutionItem)
                .WithMany(x => x.ConflictMembers)
                .HasForeignKey(x => x.ResolutionItemId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ResolutionItemConflicts_ResolutionItems");
                
            builder.HasOne(x => x.BoardMember)
                .WithMany(x => x.ConflictItems)
                .HasForeignKey(x => x.BoardMemberId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_ResolutionItemConflicts_BoardMembers");
            
            // Indexes for performance
            builder.HasIndex(x => x.ResolutionItemId)
                .HasDatabaseName("IX_ResolutionItemConflicts_ResolutionItemId");
                
            builder.HasIndex(x => x.BoardMemberId)
                .HasDatabaseName("IX_ResolutionItemConflicts_BoardMemberId");
                
            builder.HasIndex(x => new { x.ResolutionItemId, x.BoardMemberId })
                .IsUnique()
                .HasDatabaseName("IX_ResolutionItemConflicts_Item_Member_Unique")
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
