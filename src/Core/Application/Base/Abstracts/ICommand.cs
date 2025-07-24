using MediatR;
namespace Application.Base.Abstracts
{
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}
