using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstraction.Constants.ModulePermissions
{
    /// <summary>
    /// Permission constants for BoardMember module operations
    /// Defines role-based access control permissions for board member management
    /// Based on Sprint.md requirements and Clean Architecture patterns
    /// </summary>
    public static class BoardMemberPermission
    {
        /// <summary>
        /// Permission to view individual board member details
        /// </summary>
        public const string View = "BoardMember.View";

        /// <summary>
        /// Permission to list board members with pagination and filtering
        /// </summary>
        public const string List = "BoardMember.List";

        /// <summary>
        /// Permission to create new board members
        /// </summary>
        public const string Create = "BoardMember.Create";

        /// <summary>
        /// Permission to edit existing board members
        /// </summary>
        public const string Edit = "BoardMember.Edit";

        /// <summary>
        /// Permission to delete board members
        /// </summary>
        public const string Delete = "BoardMember.Delete";
    }
}
