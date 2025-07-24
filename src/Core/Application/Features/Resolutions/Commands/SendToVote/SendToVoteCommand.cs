using Abstraction.Base.Response;
using Application.Base.Abstracts;

namespace Application.Features.Resolutions.Commands.SendToVote
{
    /// <summary>
    /// Command for sending a confirmed resolution to vote
    /// Based on Sprint.md requirements (JDWA-569)
    /// Legal Council/Board Secretary can send confirmed resolutions to vote
    /// </summary>
    public class SendToVoteCommand : ICommand<BaseResponse<string>>
    {
        /// <summary>
        /// Resolution identifier to send to vote
        /// </summary>
        public int Id { get; set; }
    }
}
