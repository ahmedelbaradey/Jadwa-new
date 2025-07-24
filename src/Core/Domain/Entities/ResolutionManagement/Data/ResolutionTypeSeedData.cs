namespace Domain.Entities.ResolutionManagement
{
    /// <summary>
    /// Static class containing seed data for ResolutionType entity
    /// Based on requirements in Sprint.md for predefined resolution types
    /// </summary>
    public static class ResolutionTypeSeedData
    {
        /// <summary>
        /// Gets the predefined resolution types as specified in Sprint.md requirements
        /// These should be seeded into the database during initial setup
        /// </summary>
        /// <returns>Collection of ResolutionType entities with predefined data</returns>
        public static IEnumerable<ResolutionType> GetSeedData()
        {
            return new List<ResolutionType>
            {
                new ResolutionType
                {
                    Id = 1,
                    NameAr = "استحواذ",
                    NameEn = "Acquisition",
                    DisplayOrder = 1,
                    IsActive = true
                },
                new ResolutionType
                {
                    Id = 2,
                    NameAr = "تخارج",
                    NameEn = "Exit",
                    DisplayOrder = 2,
                    IsActive = true
                },
                new ResolutionType
                {
                    Id = 3,
                    NameAr = "بيع",
                    NameEn = "Sale",
                    DisplayOrder = 3,
                    IsActive = true
                },
                new ResolutionType
                {
                    Id = 4,
                    NameAr = "توزيع أرباح",
                    NameEn = "Profit Distribution",
                    DisplayOrder = 4,
                    IsActive = true
                },
                new ResolutionType
                {
                    Id = 5,
                    NameAr = "تمديد مدة الصندوق",
                    NameEn = "Fund Extension",
                    DisplayOrder = 5,
                    IsActive = true
                },
                new ResolutionType
                {
                    Id = 6,
                    NameAr = "تعديل شروط واحكام الصندوق",
                    NameEn = "Fund Terms and Conditions Amendment",
                    DisplayOrder = 6,
                    IsActive = true
                },
                new ResolutionType
                {
                    Id = 7,
                    NameAr = "الموافقة على القوائم المالية",
                    NameEn = "Financial Statements Approval",
                    DisplayOrder = 7,
                    IsActive = true
                },
                new ResolutionType
                {
                    Id = 8,
                    NameAr = "تعيين مقدمي خدمات",
                    NameEn = "Service Providers Appointment",
                    DisplayOrder = 8,
                    IsActive = true
                },
                new ResolutionType
                {
                    Id = 9,
                    NameAr = "موافقة على شروط واحكام الصندوق",
                    NameEn = "Fund Terms and Conditions Approval",
                    DisplayOrder = 9,
                    IsActive = true
                },
                new ResolutionType
                {
                    Id = 10,
                    NameAr = "أخرى",
                    NameEn = "Other",
                    DisplayOrder = 10,
                    IsActive = true
                }
            };
        }

        /// <summary>
        /// Gets the ID of the "Other" resolution type
        /// Used for validation when custom type name is required
        /// </summary>
        public static int OtherResolutionTypeId => 10;

        /// <summary>
        /// Checks if a resolution type ID represents the "Other" type
        /// </summary>
        /// <param name="resolutionTypeId">Resolution type ID to check</param>
        /// <returns>True if it's the "Other" type, false otherwise</returns>
        public static bool IsOtherType(int resolutionTypeId)
        {
            return resolutionTypeId == OtherResolutionTypeId;
        }
    }
}
