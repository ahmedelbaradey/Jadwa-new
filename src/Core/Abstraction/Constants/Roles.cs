using System.ComponentModel;
using System.Reflection;

namespace Abstraction.Constants
{
    public enum Roles
    {
        [Description("None")]
        None,

        [Description("مشرف عام")]
        SuperAdmin,

        [Description("مشرف")]
        Admin,

        [Description("مستخدم أساسي")]
        Basic,

        [Description("مستخدم")]
        User,

        [Description("مدير صندوق")]
        FundManager,

        [Description("المستشار القانوني")]
        LegalCouncil,

        [Description("سكرتير المجلس")]
        BoardSecretary,

        [Description("عضو مجلس الإدارة")]
        BoardMember,

        [Description("مدير المالية")]
        FinanceController,

        [Description("مدير الادارة")]
        ComplianceLegalManagingDirector,

        [Description("مدير العقارات")]
        HeadOfRealEstate,

        [Description("مدير صندوق")]
        AssociatedFundManager
    }

    /// <summary>
    /// Helper class for role validation and comparison
    /// Handles the fact that roles are stored in lowercase in the database
    /// but need to be compared against enum values
    /// </summary>
    public static class RoleHelper
    {
        // Role names as stored in database (lowercase)
        public const string FundManager = "fundmanager";
        public const string LegalCouncil = "legalcouncil";
        public const string BoardSecretary = "boardsecretary";
        public const string BoardMember = "boardmember";
        public const string SuperAdmin = "superadmin";
        public const string Admin = "admin";
        public const string Basic = "basic";
        public const string User = "user";
 

        public const string FinanceController = "financecontroller";
        public const string ComplianceLegalManagingDirector = "compliancelegalmanagingdirector";
        public const string HeadOfRealEstate = "headofrealestate";
        public const string AssociateFundManager = "associatefundmanager";
        /// <summary>
        /// Checks if user has Fund Manager role
        /// </summary>
        public static bool IsFundManager(IList<string> userRoles)
        {
            return userRoles.Any(role => string.Equals(role, FundManager, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Checks if user has Legal Council role
        /// </summary>
        public static bool IsLegalCouncil(IList<string> userRoles)
        {
            return userRoles.Any(role => string.Equals(role, LegalCouncil, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Checks if user has Board Secretary role
        /// </summary>
        public static bool IsBoardSecretary(IList<string> userRoles)
        {
            return userRoles.Any(role => string.Equals(role, BoardSecretary, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Checks if user has Board Member role
        /// </summary>
        public static bool IsBoardMember(IList<string> userRoles)
        {
            return userRoles.Any(role => string.Equals(role, BoardMember, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Checks if user has any authorized role for resolution operations
        /// </summary>
        public static bool HasAuthorizedRole(IList<string> userRoles)
        {
            return IsFundManager(userRoles) || IsLegalCouncil(userRoles) || IsBoardSecretary(userRoles);
        }
    }
}