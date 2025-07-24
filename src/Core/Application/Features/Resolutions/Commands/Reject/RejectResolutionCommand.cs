using Abstraction.Base.Response;
using Application.Base.Abstracts;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Resolutions.Commands.Reject
{
    /// <summary>
    /// Command for rejecting a resolution waiting for confirmation
    /// Based on Sprint.md requirements (JDWA-570)
    /// Fund Manager can reject resolutions with status "WaitingForConfirmation"
    /// </summary>
    public class RejectResolutionCommand : ICommand<BaseResponse<string>>
    {
        /// <summary>
        /// Resolution identifier to reject
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Reason for rejection (required)
        /// Based on Sprint.md requirements - rejection reason popup
        /// </summary>
        [Required]
        [StringLength(500, MinimumLength = 10)]
        public string RejectionReason { get; set; } = string.Empty;
    }
}
