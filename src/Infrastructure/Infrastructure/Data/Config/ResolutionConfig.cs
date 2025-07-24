using Domain.Entities.ResolutionManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    /// <summary>
    /// Entity Framework configuration for Resolution entity
    /// Configures table structure, relationships, and constraints
    /// </summary>
    public class ResolutionConfig : IEntityTypeConfiguration<Resolution>
    {
        public void Configure(EntityTypeBuilder<Resolution> builder)
        {
            // Table configuration
            builder.ToTable("Resolutions");
            
            // Primary key
            builder.HasKey(x => x.Id);
            
            // Properties configuration
            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("Auto-generated resolution code (fund code/resolution year/resolution no.)");
                
            builder.Property(x => x.ResolutionDate)
                .IsRequired()
                .HasComment("Date of the resolution");
                
            builder.Property(x => x.Description)
                .HasMaxLength(500)
                .HasComment("Description of the resolution (optional, max 500 characters)");
                
            builder.Property(x => x.ResolutionTypeId)
                .IsRequired()
                .HasComment("Foreign key reference to ResolutionType entity");
                
            builder.Property(x => x.NewType)
                .HasMaxLength(200)
                .HasComment("Custom resolution type name when 'Other' is selected");
                
            builder.Property(x => x.AttachmentId)
                .IsRequired()
                .HasComment("Foreign key reference to main Attachment entity");
                
            builder.Property(x => x.VotingType)
                .IsRequired()
                .HasConversion<int>()
                .HasComment("Voting methodology (AllMembers=1, Majority=2)");
                
            builder.Property(x => x.MemberVotingResult)
                .IsRequired()
                .HasConversion<int>()
                .HasDefaultValue(MemberVotingResult.MajorityOfItems)
                .HasComment("How member voting results are calculated");
                
            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<int>()
                .HasDefaultValue(ResolutionStatusEnum.Draft)
                .HasComment("Current status of the resolution");
                
            builder.Property(x => x.FundId)
                .IsRequired()
                .HasComment("Foreign key reference to Fund entity");
                
            builder.Property(x => x.ParentResolutionId)
                .HasComment("Parent resolution identifier for tracking relationships");
                
            builder.Property(x => x.OldResolutionCode)
                .HasMaxLength(50)
                .HasComment("Old resolution code when this is an update to existing resolution");
            
            // Relationships configuration
            builder.HasOne(x => x.Fund)
                .WithMany(f => f.Resolutions)
                .HasForeignKey(x => x.FundId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Resolutions_Funds");

            builder.HasOne(x => x.ResolutionType)
                .WithMany(rt => rt.Resolutions)
                .HasForeignKey(x => x.ResolutionTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Resolutions_ResolutionTypes");
                
            builder.HasOne(x => x.Attachment)
                .WithMany()
                .HasForeignKey(x => x.AttachmentId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Resolutions_Attachments");
                
            builder.HasOne(x => x.ParentResolution)
                .WithMany(x => x.ChildResolutions)
                .HasForeignKey(x => x.ParentResolutionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Resolutions_ParentResolution");
            
            // Indexes for performance
            builder.HasIndex(x => x.Code)
                .HasFilter("[IsDeleted] = 0")
                .IsUnique()
                .HasDatabaseName("IX_Resolutions_Code_Unique");
                
            builder.HasIndex(x => x.FundId)
                .HasDatabaseName("IX_Resolutions_FundId");
                
            builder.HasIndex(x => x.Status)
                .HasDatabaseName("IX_Resolutions_Status");
                
            builder.HasIndex(x => new { x.FundId, x.Status })
                .HasDatabaseName("IX_Resolutions_Fund_Status");
                
            builder.HasIndex(x => x.ResolutionDate)
                .HasDatabaseName("IX_Resolutions_Date");
                
            builder.HasIndex(x => x.ParentResolutionId)
                .HasDatabaseName("IX_Resolutions_ParentId");
            
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
