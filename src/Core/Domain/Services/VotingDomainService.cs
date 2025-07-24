using Domain.Entities.FundManagement;
using Domain.Entities.ResolutionManagement;
using Domain.Entities.Shared;

namespace Domain.Services
{
    /// <summary>
    /// Domain service for voting business rules and operations
    /// Encapsulates complex business logic related to resolution voting
    /// Based on voting requirements for resolution management system
    /// </summary>
    public class VotingDomainService
    {
        /// <summary>
        /// Validates if a board member can vote on a resolution
        /// </summary>
        /// <param name="boardMember">Board member attempting to vote</param>
        /// <param name="resolution">Resolution being voted on</param>
        /// <returns>Validation result with success status and error message</returns>
        public static (bool CanVote, string ErrorMessage) CanMemberVoteOnResolution(
            BoardMember boardMember, 
            Resolution resolution)
        {
            if (!boardMember.IsActive)
            {
                return (false, "Board member is not active and cannot vote.");
            }

            if (resolution.Status != ResolutionStatusEnum.VotingInProgress)
            {
                return (false, "Resolution is not in voting status.");
            }

            if (boardMember.FundId != resolution.FundId)
            {
                return (false, "Board member does not belong to the fund of this resolution.");
            }

            return (true, string.Empty);
        }

        /// <summary>
        /// Validates if a board member can vote on a specific resolution item
        /// </summary>
        /// <param name="boardMember">Board member attempting to vote</param>
        /// <param name="resolutionItem">Resolution item being voted on</param>
        /// <param name="conflicts">List of conflicts for this item</param>
        /// <returns>Validation result with success status and error message</returns>
        public static (bool CanVote, string ErrorMessage) CanMemberVoteOnItem(
            BoardMember boardMember,
            ResolutionItem resolutionItem,
            IEnumerable<ResolutionItemConflict> conflicts)
        {
            // First check if member can vote on the resolution
            var resolutionCheck = CanMemberVoteOnResolution(boardMember, resolutionItem.Resolution);
            if (!resolutionCheck.CanVote)
            {
                return resolutionCheck;
            }

            // Check for conflict of interest
            if (resolutionItem.HasConflict)
            {
                var hasConflict = conflicts.Any(c => c.BoardMemberId == boardMember.Id);
                if (hasConflict)
                {
                    return (false, "Board member has a conflict of interest with this resolution item.");
                }
            }

            return (true, string.Empty);
        }

        /// <summary>
        /// Calculates voting results for a resolution based on voting methodology
        /// </summary>
        /// <param name="resolution">Resolution to calculate results for</param>
        /// <param name="votes">All active votes for the resolution</param>
        /// <param name="eligibleMembers">All eligible board members for voting</param>
        /// <returns>Voting result with approval status and details</returns>
        public static VotingResult CalculateResolutionResult(
            Resolution resolution,
            IEnumerable<ResolutionVote> votes,
            IEnumerable<BoardMember> eligibleMembers)
        {
            var result = new VotingResult();
            
            // Get resolution-level votes (not item-specific)
            var resolutionVotes = votes.Where(v => v.ResolutionItemId == null && v.IsActive).ToList();
            
            // If resolution has items, calculate based on item voting
            if (resolution.ResolutionItems.Any())
            {
                return CalculateItemBasedResult(resolution, votes, eligibleMembers);
            }

            // Calculate based on resolution-level voting
            return CalculateDirectResolutionResult(resolution, resolutionVotes, eligibleMembers);
        }

        /// <summary>
        /// Calculates voting results based on resolution items
        /// </summary>
        private static VotingResult CalculateItemBasedResult(
            Resolution resolution,
            IEnumerable<ResolutionVote> votes,
            IEnumerable<BoardMember> eligibleMembers)
        {
            var result = new VotingResult();
            var itemResults = new List<ItemVotingResult>();

            foreach (var item in resolution.ResolutionItems)
            {
                var itemVotes = votes.Where(v => v.ResolutionItemId == item.Id && v.IsActive).ToList();
                var eligibleForItem = GetEligibleMembersForItem(item, eligibleMembers);
                
                var itemResult = CalculateItemResult(item, itemVotes, eligibleForItem, resolution.VotingType);
                itemResults.Add(itemResult);
            }

            // Apply member voting result logic (AllItems vs MajorityOfItems)
            if (resolution.MemberVotingResult == MemberVotingResult.AllItems)
            {
                result.IsApproved = itemResults.All(r => r.IsApproved);
            }
            else // MajorityOfItems
            {
                var approvedCount = itemResults.Count(r => r.IsApproved);
                result.IsApproved = approvedCount > (itemResults.Count / 2.0);
            }

            result.ItemResults = itemResults;
            result.TotalEligibleVoters = eligibleMembers.Count();
            result.TotalVotesCast = votes.Count(v => v.IsActive);

            return result;
        }

        /// <summary>
        /// Calculates voting results for direct resolution voting (no items)
        /// </summary>
        private static VotingResult CalculateDirectResolutionResult(
            Resolution resolution,
            IList<ResolutionVote> resolutionVotes,
            IEnumerable<BoardMember> eligibleMembers)
        {
            var result = new VotingResult();
            var eligibleCount = eligibleMembers.Count();
            var approveVotes = resolutionVotes.Count(v => v.VoteValue == VoteValue.Approve);
            var rejectVotes = resolutionVotes.Count(v => v.VoteValue == VoteValue.Reject);

            if (resolution.VotingType == VotingType.AllMembers)
            {
                // Unanimous approval required
                result.IsApproved = approveVotes == eligibleCount && rejectVotes == 0;
            }
            else // Majority
            {
                // Majority approval required
                result.IsApproved = approveVotes > (eligibleCount / 2.0);
            }

            result.TotalEligibleVoters = eligibleCount;
            result.TotalVotesCast = resolutionVotes.Count;
            result.ApproveVotes = approveVotes;
            result.RejectVotes = rejectVotes;
            result.AbstainVotes = resolutionVotes.Count(v => v.VoteValue == VoteValue.Abstain);

            return result;
        }

        /// <summary>
        /// Gets eligible members for voting on a specific item (excluding conflicts)
        /// </summary>
        private static IEnumerable<BoardMember> GetEligibleMembersForItem(
            ResolutionItem item,
            IEnumerable<BoardMember> allMembers)
        {
            if (!item.HasConflict)
            {
                return allMembers;
            }

            var conflictMemberIds = item.ConflictMembers.Select(c => c.BoardMemberId).ToHashSet();
            return allMembers.Where(m => !conflictMemberIds.Contains(m.Id));
        }

        /// <summary>
        /// Calculates voting result for a single resolution item
        /// </summary>
        private static ItemVotingResult CalculateItemResult(
            ResolutionItem item,
            IList<ResolutionVote> itemVotes,
            IEnumerable<BoardMember> eligibleMembers,
            VotingType votingMethodology)
        {
            var result = new ItemVotingResult
            {
                ItemId = item.Id,
                ItemTitle = item.Title
            };

            var eligibleCount = eligibleMembers.Count();
            var approveVotes = itemVotes.Count(v => v.VoteValue == VoteValue.Approve);
            var rejectVotes = itemVotes.Count(v => v.VoteValue == VoteValue.Reject);

            if (votingMethodology == VotingType.AllMembers)
            {
                result.IsApproved = approveVotes == eligibleCount && rejectVotes == 0;
            }
            else // Majority
            {
                result.IsApproved = approveVotes > (eligibleCount / 2.0);
            }

            result.EligibleVoters = eligibleCount;
            result.VotesCast = itemVotes.Count;
            result.ApproveVotes = approveVotes;
            result.RejectVotes = rejectVotes;
            result.AbstainVotes = itemVotes.Count(v => v.VoteValue == VoteValue.Abstain);

            return result;
        }

        /// <summary>
        /// Creates a new voting session for a resolution
        /// </summary>
        /// <returns>New voting session ID</returns>
        public static Guid CreateVotingSession()
        {
            return Guid.NewGuid();
        }

        /// <summary>
        /// Suspends all active votes for a resolution (used when resolution is edited during voting)
        /// </summary>
        /// <param name="votes">All votes for the resolution</param>
        /// <returns>Number of votes suspended</returns>
        public static int SuspendActiveVotes(IEnumerable<ResolutionVote> votes)
        {
            var activeVotes = votes.Where(v => v.IsActive).ToList();
            foreach (var vote in activeVotes)
            {
                vote.IsActive = false;
            }
            return activeVotes.Count;
        }
    }

    /// <summary>
    /// Represents the result of voting calculation for a resolution
    /// </summary>
    public class VotingResult
    {
        public bool IsApproved { get; set; }
        public int TotalEligibleVoters { get; set; }
        public int TotalVotesCast { get; set; }
        public int ApproveVotes { get; set; }
        public int RejectVotes { get; set; }
        public int AbstainVotes { get; set; }
        public IEnumerable<ItemVotingResult> ItemResults { get; set; } = new List<ItemVotingResult>();
    }

    /// <summary>
    /// Represents the voting result for a single resolution item
    /// </summary>
    public class ItemVotingResult
    {
        public int ItemId { get; set; }
        public string ItemTitle { get; set; } = string.Empty;
        public bool IsApproved { get; set; }
        public int EligibleVoters { get; set; }
        public int VotesCast { get; set; }
        public int ApproveVotes { get; set; }
        public int RejectVotes { get; set; }
        public int AbstainVotes { get; set; }
    }
}
