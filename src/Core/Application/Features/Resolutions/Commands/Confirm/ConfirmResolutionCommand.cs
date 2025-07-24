using Abstraction.Base.Response;
using Application.Base.Abstracts;

namespace Application.Features.Resolutions.Commands.Confirm
{
    /// <summary>
    /// Command for confirming a resolution waiting for confirmation
    /// Based on Sprint.md requirements (JDWA-570)
    /// Fund Manager can confirm resolutions with status "WaitingForConfirmation"
    /// </summary>
    public class ConfirmResolutionCommand : ICommand<BaseResponse<string>>
    {
        /// <summary>
        /// Resolution identifier to confirm
        /// </summary>
        public int Id { get; set; }
    }
}
