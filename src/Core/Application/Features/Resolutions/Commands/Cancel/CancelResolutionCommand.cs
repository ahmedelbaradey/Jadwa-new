using Abstraction.Base.Response;
using Application.Base.Abstracts;

namespace Application.Features.Resolutions.Commands.Cancel
{
    /// <summary>
    /// Command for cancelling a pending resolution
    /// Based on Sprint.md requirements (JDWA-508)
    /// </summary>
    public class CancelResolutionCommand : ICommand<BaseResponse<string>>
    {
        public int Id { get; set; }
    }
}
