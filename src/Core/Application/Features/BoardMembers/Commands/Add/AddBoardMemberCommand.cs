using Abstraction.Base.Response;
using Application.Base.Abstracts;
using Application.Features.BoardMembers.Dtos;


namespace Application.Features.BoardMembers.Commands.Add
{
    /// <summary>
    /// Command for adding a new board member to a fund
    /// Implements CQRS pattern using MediatR
    /// Based on requirements in Sprint.md (JDWA-596)
    /// </summary>
    public record AddBoardMemberCommand : AddBoardMemberDto, ICommand<BaseResponse<string>>
    {
    }
}
