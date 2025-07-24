using MediatR;


namespace Application.Base.Abstracts
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
    {
    }
}
