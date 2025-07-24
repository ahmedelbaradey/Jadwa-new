using Domain.Entities.FundManagement;

namespace Domain.Services
{
    /// <summary>
    /// Domain service for board member business rules and operations
    /// Encapsulates complex business logic related to board member management
    /// Based on requirements in Sprint.md for board member business rules
    /// </summary>
    public class BoardMemberDomainService
    {
        /// <summary>
        /// Maximum number of independent board members allowed per fund
        /// Business rule from Sprint.md (JDWA-596)
        /// </summary>
        public const int MaxIndependentMembers = 14;

        /// <summary>
        /// Number of independent members required to activate a fund
        /// Business rule from Sprint.md (JDWA-596)
        /// </summary>
        public const int RequiredIndependentMembersForActivation = 2;

        /// <summary>
        /// Maximum number of chairmen allowed per fund
        /// Business rule from Sprint.md (JDWA-596)
        /// </summary>
        public const int MaxChairmenPerFund = 1;

        /// <summary>
        /// Validates if a new independent board member can be added to the fund
        /// </summary>
        /// <param name="existingBoardMembers">Current board members of the fund</param>
        /// <param name="newMemberType">Type of the new member being added</param>
        /// <returns>True if the member can be added, false otherwise</returns>
        public static bool CanAddIndependentMember(IEnumerable<BoardMember> existingBoardMembers, BoardMemberType newMemberType)
        {
            if (newMemberType != BoardMemberType.Independent)
                return true; // No limit for non-independent members

            var currentIndependentCount = existingBoardMembers
                .Count(m => m.MemberType == BoardMemberType.Independent && m.IsActive);

            return currentIndependentCount < MaxIndependentMembers;
        }

        /// <summary>
        /// Validates if a new chairman can be added to the fund
        /// </summary>
        /// <param name="existingBoardMembers">Current board members of the fund</param>
        /// <param name="isNewMemberChairman">Whether the new member will be chairman</param>
        /// <returns>True if the chairman can be added, false otherwise</returns>
        public static bool CanAddChairman(IEnumerable<BoardMember> existingBoardMembers, bool isNewMemberChairman)
        {
            if (!isNewMemberChairman)
                return true; // Not adding a chairman

            var currentChairmenCount = existingBoardMembers
                .Count(m => m.IsChairman && m.IsActive);

            return currentChairmenCount < MaxChairmenPerFund;
        }

        /// <summary>
        /// Checks if the fund should be activated based on independent member count
        /// </summary>
        /// <param name="boardMembers">All board members of the fund</param>
        /// <returns>True if fund should be activated, false otherwise</returns>
        public static bool ShouldActivateFund(IEnumerable<BoardMember> boardMembers)
        {
            var activeIndependentCount = boardMembers
                .Count(m => m.MemberType == BoardMemberType.Independent && m.IsActive);

            return activeIndependentCount >= RequiredIndependentMembersForActivation;
        }

        /// <summary>
        /// Gets the count of active independent board members
        /// </summary>
        /// <param name="boardMembers">Board members to count</param>
        /// <returns>Number of active independent board members</returns>
        public static int GetActiveIndependentMemberCount(IEnumerable<BoardMember> boardMembers)
        {
            return boardMembers.Count(m => m.MemberType == BoardMemberType.Independent && m.IsActive);
        }

        /// <summary>
        /// Gets the count of active chairmen
        /// </summary>
        /// <param name="boardMembers">Board members to count</param>
        /// <returns>Number of active chairmen</returns>
        public static int GetActiveChairmenCount(IEnumerable<BoardMember> boardMembers)
        {
            return boardMembers.Count(m => m.IsChairman && m.IsActive);
        }

        /// <summary>
        /// Validates all business rules for adding a new board member
        /// </summary>
        /// <param name="existingBoardMembers">Current board members of the fund</param>
        /// <param name="newMemberType">Type of the new member</param>
        /// <param name="isNewMemberChairman">Whether the new member will be chairman</param>
        /// <returns>Validation result with success status and error message</returns>
        public static (bool IsValid, string ErrorMessage) ValidateNewBoardMember(
            IEnumerable<BoardMember> existingBoardMembers, 
            BoardMemberType newMemberType, 
            bool isNewMemberChairman)
        {
            // Check independent member limit
            if (!CanAddIndependentMember(existingBoardMembers, newMemberType))
            {
                return (false, "Maximum number of independent board members (14) has been reached for this fund.");
            }

            // Check chairman limit
            if (!CanAddChairman(existingBoardMembers, isNewMemberChairman))
            {
                return (false, "This fund already has a chairman. Each fund can have only one chairman.");
            }

            return (true, string.Empty);
        }
    }
}
