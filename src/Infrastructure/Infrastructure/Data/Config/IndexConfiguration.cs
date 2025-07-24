using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.FundManagement;
using Domain.Entities.Users;
using Domain.Entities.Startegies;
using Domain.Entities.Shared;
using Domain.Entities.Notifications;

namespace Infrastructure.Data.Configurations
{
    /// <summary>
    /// Database indexing configuration for optimal query performance
    /// Following SQL Server indexing best practices
    /// </summary>
    public static class IndexConfiguration
    {
        public static void ConfigureIndexes(this ModelBuilder modelBuilder)
        {
            ConfigureFundIndexes(modelBuilder);
            ConfigureFundRelationshipIndexes(modelBuilder);
            ConfigureUserIndexes(modelBuilder);
            ConfigureStrategyIndexes(modelBuilder);
            ConfigureNotificationIndexes(modelBuilder);
            ConfigureAuditIndexes(modelBuilder);
        }

        #region Fund Indexes
        private static void ConfigureFundIndexes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Fund>(entity =>
            {
                // 1. Fund Search Optimization - Composite index for search functionality
                entity.HasIndex(f => new { f.Name, f.InitiationDate, f.StrategyId })
                    .HasDatabaseName("IX_Funds_Search_Composite")
                    .HasFilter("[IsDeleted] = 0");

                // 2. Fund Name Search - Optimized for LIKE queries
                entity.HasIndex(f => f.Name)
                    .HasDatabaseName("IX_Funds_Name")
                    .HasFilter("[IsDeleted] = 0");

                // 3. Initiation Date Range Queries
                entity.HasIndex(f => f.InitiationDate)
                    .HasDatabaseName("IX_Funds_InitiationDate")
                    .HasFilter("[IsDeleted] = 0");

                // 4. Strategy-based filtering
                entity.HasIndex(f => f.StrategyId)
                    .HasDatabaseName("IX_Funds_StrategyId")
                    .HasFilter("[IsDeleted] = 0");

                // 5. Legal Council access optimization
                entity.HasIndex(f => f.LegalCouncilId)
                    .HasDatabaseName("IX_Funds_LegalCouncilId")
                    .HasFilter("[IsDeleted] = 0");

                // 6. Exit Date queries (for active/exited funds)
                entity.HasIndex(f => f.ExitDate)
                    .HasDatabaseName("IX_Funds_ExitDate")
                    .HasFilter("[IsDeleted] = 0");

                // 7. Composite index for date range + strategy (common search pattern)
                entity.HasIndex(f => new { f.StrategyId, f.InitiationDate })
                    .HasDatabaseName("IX_Funds_Strategy_InitiationDate")
                    .HasFilter("[IsDeleted] = 0");

                // 8. Audit trail optimization
                entity.HasIndex(f => new { f.CreatedAt, f.CreatedBy })
                    .HasDatabaseName("IX_Funds_Audit_Created")
                    .HasFilter("[IsDeleted] = 0");
            });
        }
        #endregion

        #region Fund Relationship Indexes
        private static void ConfigureFundRelationshipIndexes(ModelBuilder modelBuilder)
        {
            // Fund Managers - Role-based access optimization
            modelBuilder.Entity<FundManager>(entity =>
            {
                // Primary lookup: Find funds by manager
                entity.HasIndex(fm => fm.UserId)
                    .HasDatabaseName("IX_FundManagers_UserId")
                    .HasFilter("[IsDeleted] = 0");

                // Reverse lookup: Find managers by fund
                entity.HasIndex(fm => fm.FundId)
                    .HasDatabaseName("IX_FundManagers_FundId")
                    .HasFilter("[IsDeleted] = 0");

                // Composite for unique constraint and performance
                entity.HasIndex(fm => new { fm.FundId, fm.UserId })
                    .HasDatabaseName("IX_FundManagers_Fund_User_Unique")
                    .IsUnique()
                    .HasFilter("[IsDeleted] = 0");
            });

            // Fund Board Secretaries - Role-based access optimization
            modelBuilder.Entity<FundBoardSecretary>(entity =>
            {
                // Primary lookup: Find funds by secretary
                entity.HasIndex(fbs => fbs.UserId)
                    .HasDatabaseName("IX_FundBoardSecretaries_UserId")
                    .HasFilter("[IsDeleted] = 0");

                // Reverse lookup: Find secretaries by fund
                entity.HasIndex(fbs => fbs.FundId)
                    .HasDatabaseName("IX_FundBoardSecretaries_FundId")
                    .HasFilter("[IsDeleted] = 0");

                // Composite for unique constraint and performance
                entity.HasIndex(fbs => new { fbs.FundId, fbs.UserId })
                    .HasDatabaseName("IX_FundBoardSecretaries_Fund_User_Unique")
                    .IsUnique()
                    .HasFilter("[IsDeleted] = 0");
            });

            // Fund Status History - Status tracking optimization
            modelBuilder.Entity<FundStatusHistory>(entity =>
            {
                // Find latest status by fund (most common query)
                entity.HasIndex(fsh => new { fsh.FundId, fsh.CreatedAt })
                    .HasDatabaseName("IX_FundStatusHistory_Fund_CreatedAt_DESC")
                    .IsDescending(false, true)
                    .HasFilter("[IsDeleted] = 0");

                // Status-based queries
                entity.HasIndex(fsh => fsh.StatusHistoryId)
                    .HasDatabaseName("IX_FundStatusHistory_StatusId")
                    .HasFilter("[IsDeleted] = 0");

                // Audit trail
                entity.HasIndex(fsh => fsh.CreatedBy)
                    .HasDatabaseName("IX_FundStatusHistory_CreatedBy")
                    .HasFilter("[IsDeleted] = 0");
            });
        }
        #endregion

        #region User and Identity Indexes
        private static void ConfigureUserIndexes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                // Email lookup optimization (already exists in Identity, but ensuring)
                entity.HasIndex(u => u.Email)
                    .HasDatabaseName("IX_Users_Email")
                    .IsUnique();

                // Username lookup optimization
                entity.HasIndex(u => u.UserName)
                    .HasDatabaseName("IX_Users_UserName")
                    .IsUnique();

                // Full name search for user selection dropdowns
                entity.HasIndex(u => u.FullName)
                    .HasDatabaseName("IX_Users_FullName");

                // Phone number lookup
                entity.HasIndex(u => u.PhoneNumber)
                    .HasDatabaseName("IX_Users_PhoneNumber");
            });
        }
        #endregion

        #region Strategy Indexes
        private static void ConfigureStrategyIndexes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Strategy>(entity =>
            {
                // Arabic name search
                entity.HasIndex(s => s.NameAr)
                    .HasDatabaseName("IX_Strategies_NameAr")
                    .HasFilter("[IsDeleted] = 0");

                // English name search
                entity.HasIndex(s => s.NameEn)
                    .HasDatabaseName("IX_Strategies_NameEn")
                    .HasFilter("[IsDeleted] = 0");

                // Audit trail
                entity.HasIndex(s => s.CreatedAt)
                    .HasDatabaseName("IX_Strategies_CreatedAt")
                    .HasFilter("[IsDeleted] = 0");
            });
        }
        #endregion

        #region Notification Indexes
        private static void ConfigureNotificationIndexes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>(entity =>
            {
                // Pending notifications query optimization
                entity.HasIndex(n => new { n.IsSent, n.CreatedAt })
                    .HasDatabaseName("IX_Notifications_IsSent_CreatedAt");

                // User-specific notifications
                entity.HasIndex(n => n.UserId)
                    .HasDatabaseName("IX_Notifications_UserId");

                // Fund-related notifications
                entity.HasIndex(n => n.FundId)
                    .HasDatabaseName("IX_Notifications_FundId");
            });
        }
        #endregion

        #region Audit and Base Entity Indexes
        private static void ConfigureAuditIndexes(ModelBuilder modelBuilder)
        {
            // Entities that inherit from FullAuditedEntity (have all audit properties)
            var fullAuditedEntityTypes = new[]
            {
                typeof(Fund),
                typeof(FundManager),
                typeof(FundBoardSecretary),
                typeof(FundStatusHistory),
                typeof(Strategy)
            };

            // Entities that inherit from CreationAuditedEntity only (no UpdatedAt/IsDeleted)
            var creationAuditedEntityTypes = new[]
            {
                typeof(Notification)
            };

            // Configure indexes for FullAuditedEntity types
            foreach (var entityType in fullAuditedEntityTypes)
            {
                var entityBuilder = modelBuilder.Entity(entityType);
                var entityName = entityType.Name;

                // Created date index for chronological queries
                entityBuilder.HasIndex("CreatedAt")
                    .HasDatabaseName($"IX_{entityName}_CreatedAt");

                // Created by index for audit trails
                entityBuilder.HasIndex("CreatedBy")
                    .HasDatabaseName($"IX_{entityName}_CreatedBy");

                // Updated date index for recent changes
                entityBuilder.HasIndex("UpdatedAt")
                    .HasDatabaseName($"IX_{entityName}_UpdatedAt");

                // Soft delete optimization
                entityBuilder.HasIndex("IsDeleted")
                    .HasDatabaseName($"IX_{entityName}_IsDeleted");
            }

            // Configure indexes for CreationAuditedEntity types (only creation audit properties)
            foreach (var entityType in creationAuditedEntityTypes)
            {
                var entityBuilder = modelBuilder.Entity(entityType);
                var entityName = entityType.Name;

                // Created date index for chronological queries
                entityBuilder.HasIndex("CreatedAt")
                    .HasDatabaseName($"IX_{entityName}_CreatedAt");

                // Created by index for audit trails
                entityBuilder.HasIndex("CreatedBy")
                    .HasDatabaseName($"IX_{entityName}_CreatedBy");
            }
        }
        #endregion
    }
}
