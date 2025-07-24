using Domain.Entities.FundManagement;
using Domain.Entities.ResolutionManagement;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Common.Helpers
{
    /// <summary>
    /// Helper class for localized display properties
    /// Provides centralized localization logic for DTOs
    /// Uses IStringLocalizer for resource-based localization
    /// </summary>
    public static class LocalizationHelper
    {
        /// <summary>
        /// Gets localized display text for board member type
        /// </summary>
        /// <param name="memberType">Board member type enum</param>
        /// <param name="localizer">String localizer instance</param>
        /// <returns>Localized member type display text</returns>
        public static string GetBoardMemberTypeDisplay(BoardMemberType memberType, IStringLocalizer<SharedResources> localizer)
        {
            return memberType switch
            {
                BoardMemberType.Independent => localizer[SharedResourcesKey.BoardMemberTypeIndependent],
                BoardMemberType.NotIndependent => localizer[SharedResourcesKey.BoardMemberTypeNotIndependent],
                _ => memberType.ToString()
            };
        }

        /// <summary>
        /// Gets localized display text for resolution status
        /// </summary>
        /// <param name="status">Resolution status enum</param>
        /// <param name="localizer">String localizer instance</param>
        /// <returns>Localized status display text</returns>
        public static string GetResolutionStatusDisplay(ResolutionStatusEnum status, IStringLocalizer<SharedResources> localizer)
        {
            return status switch
            {
                ResolutionStatusEnum.Draft => localizer[SharedResourcesKey.ResolutionStatusDraft],
                ResolutionStatusEnum.Pending => localizer[SharedResourcesKey.ResolutionStatusPending],
                ResolutionStatusEnum.CompletingData => localizer[SharedResourcesKey.ResolutionStatusCompletingData],
                ResolutionStatusEnum.WaitingForConfirmation => localizer[SharedResourcesKey.ResolutionStatusWaitingForConfirmation],
                ResolutionStatusEnum.Confirmed => localizer[SharedResourcesKey.ResolutionStatusConfirmed],
                ResolutionStatusEnum.Rejected => localizer[SharedResourcesKey.ResolutionStatusRejected],
                ResolutionStatusEnum.VotingInProgress => localizer[SharedResourcesKey.ResolutionStatusVotingInProgress],
                ResolutionStatusEnum.Approved => localizer[SharedResourcesKey.ResolutionStatusApproved],
                ResolutionStatusEnum.NotApproved => localizer[SharedResourcesKey.ResolutionStatusNotApproved],
                ResolutionStatusEnum.Cancelled => localizer[SharedResourcesKey.ResolutionStatusCancelled],
                _ => status.ToString()
            };
        }

        /// <summary>
        /// Gets localized display text for vote value
        /// </summary>
        /// <param name="voteValue">Vote value enum</param>
        /// <param name="localizer">String localizer instance</param>
        /// <returns>Localized vote value display text</returns>
        public static string GetVoteValueDisplay(VoteValue voteValue, IStringLocalizer<SharedResources> localizer)
        {
            return voteValue switch
            {
                VoteValue.Approve => localizer[SharedResourcesKey.VoteDecisionApprove],
                VoteValue.Reject => localizer[SharedResourcesKey.VoteDecisionReject],
                VoteValue.Abstain => localizer[SharedResourcesKey.VoteDecisionAbstain],
                _ => voteValue.ToString()
            };
        }

        /// <summary>
        /// Gets localized display text for voting type
        /// </summary>
        /// <param name="votingType">Voting type enum</param>
        /// <param name="localizer">String localizer instance</param>
        /// <returns>Localized voting type display text</returns>
        public static string GetVotingTypeDisplay(VotingType votingType, IStringLocalizer<SharedResources> localizer)
        {
            return votingType switch
            {
                VotingType.AllMembers => localizer[SharedResourcesKey.VotingTypeAllMembers],
                VotingType.Majority => localizer[SharedResourcesKey.VotingTypeMajority],
                _ => votingType.ToString()
            };
        }

        /// <summary>
        /// Gets localized display text for member voting result
        /// </summary>
        /// <param name="memberVotingResult">Member voting result enum</param>
        /// <param name="localizer">String localizer instance</param>
        /// <returns>Localized member voting result display text</returns>
        public static string GetMemberVotingResultDisplay(MemberVotingResult memberVotingResult, IStringLocalizer<SharedResources> localizer)
        {
            return memberVotingResult switch
            {
                MemberVotingResult.AllItems => localizer[SharedResourcesKey.MemberVotingResultAllItems],
                MemberVotingResult.MajorityOfItems => localizer[SharedResourcesKey.MemberVotingResultMajorityOfItems],
                _ => memberVotingResult.ToString()
            };
        }

        /// <summary>
        /// Gets localized display text for user role names
        /// Maps database role names to localized display names based on user's preferred language
        /// </summary>
        /// <param name="roleName">Role name from database</param>
        /// <param name="localizer">String localizer instance</param>
        /// <returns>Localized role name display text</returns>
        public static string GetUserRoleDisplay(string roleName, IStringLocalizer<SharedResources> localizer)
        {
            if (string.IsNullOrEmpty(roleName))
                return roleName;

            // Convert to lowercase for consistent comparison
            var roleNameLower = roleName.ToLower();

            return roleNameLower switch
            {
                "fundmanager" => localizer[SharedResourcesKey.FundManager],
                "legalcouncil" => localizer[SharedResourcesKey.LegalCouncil],
                "boardsecretary" => localizer[SharedResourcesKey.BoardSecretary],
                "boardmember" => localizer[SharedResourcesKey.BoardMember],
                "superadmin" => localizer[SharedResourcesKey.SuperAdmin],
                "admin" => localizer[SharedResourcesKey.Admin],
                "basic" => localizer[SharedResourcesKey.Basic],
                "user" => localizer[SharedResourcesKey.User],
                "financecontroller" => localizer[SharedResourcesKey.FinanceController],
                "compliancelegalmanagingdirector" => localizer[SharedResourcesKey.ComplianceLegalManagingDirector],
                "headofrealestate" => localizer[SharedResourcesKey.HeadOfRealEstate],
                "associatefundmanager" => localizer[SharedResourcesKey.AssociateFundManager],
                _ => roleName // Return original name if no localization found
            };
        }
    }
}
