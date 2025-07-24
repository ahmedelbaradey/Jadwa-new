using Domain.Entities.ResolutionManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    /// <summary>
    /// Entity Framework configuration for ResolutionAttachment entity
    /// Configures table structure, relationships, and constraints
    /// </summary>
    public class ResolutionAttachmentConfig : IEntityTypeConfiguration<ResolutionAttachment>
    {
        public void Configure(EntityTypeBuilder<ResolutionAttachment> builder)
        {
            // Table configuration
            builder.ToTable("ResolutionAttachments");
            
            // Primary key
            builder.HasKey(x => x.Id);
            
            // Properties configuration
            builder.Property(x => x.ResolutionId)
                .IsRequired()
                .HasComment("Foreign key reference to Resolution entity");
                
            builder.Property(x => x.AttachmentId)
                .IsRequired()
                .HasComment("Foreign key reference to Attachment entity");
            
            // Relationships configuration
            builder.HasOne(x => x.Resolution)
                .WithMany(x => x.OtherAttachments)
                .HasForeignKey(x => x.ResolutionId)
                .OnDelete(DeleteBehavior.Cascade) // Allow cascade delete when resolution is deleted
                .HasConstraintName("FK_ResolutionAttachments_Resolutions");
                
            builder.HasOne(x => x.Attachment)
                .WithMany()
                .HasForeignKey(x => x.AttachmentId)
                .OnDelete(DeleteBehavior.Restrict) // Prevent attachment deletion if referenced
                .HasConstraintName("FK_ResolutionAttachments_Attachments");
            
            // Indexes for performance
            builder.HasIndex(x => x.ResolutionId)
                .HasDatabaseName("IX_ResolutionAttachments_ResolutionId");
                
            builder.HasIndex(x => x.AttachmentId)
                .HasDatabaseName("IX_ResolutionAttachments_AttachmentId");
                
            // Unique constraint to prevent duplicate resolution-attachment pairs
            builder.HasIndex(x => new { x.ResolutionId, x.AttachmentId })
                .IsUnique()
                .HasDatabaseName("IX_ResolutionAttachments_Resolution_Attachment_Unique")
                .HasFilter("([IsDeleted] = 0 OR [IsDeleted] IS NULL)");
            
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
