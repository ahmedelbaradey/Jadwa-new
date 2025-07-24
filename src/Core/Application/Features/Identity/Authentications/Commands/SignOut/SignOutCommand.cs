using Abstraction.Base.Response;
using Application.Base.Abstracts;

namespace Application.Features.Identity.Authentications.Commands.SignOut
{
    /// <summary>
    /// Command for user logout
    /// Enhanced for Sprint 3 with proper session termination and audit logging
    /// </summary>
    public record SignOutCommand : ICommand<BaseResponse<string>>
    {

    }
 
  
}
