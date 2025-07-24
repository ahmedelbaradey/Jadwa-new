using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Application.Features.Identity.Users.Queries.Responses;

namespace Application.Features.Identity.Users.Queries.GetCurrentCultureLanguage
{
    /// <summary>
    /// Query to retrieve the current culture language for the authenticated user
    /// Returns both the current culture from the request context and the user's preferred language
    /// </summary>
    public record GetCurrentCultureLanguageQuery : IQuery<BaseResponse<GetCurrentCultureLanguageResponse>>
    {
        // No parameters needed as this gets the current user's information
    }
}
