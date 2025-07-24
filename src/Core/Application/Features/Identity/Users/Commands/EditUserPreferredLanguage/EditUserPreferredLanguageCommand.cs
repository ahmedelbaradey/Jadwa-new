using Application.Base.Abstracts;
using Abstraction.Base.Response;

namespace Application.Features.Identity.Users.Commands.EditUserPreferredLanguage
{
    public record EditUserPreferredLanguageCommand : ICommand<BaseResponse<string>>
    {
        public int Id { get; set; }
      
        public string UserPreferredLanguage  { get; set; }
    }
}
