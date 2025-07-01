namespace Application.Abstraction.Command
{
    using MediatR;
    using Domain.Shared;

    public interface ICommand : IRequest<Result>
    {

    }
     
    public interface ICommand<TResponse> : IRequest<Result<TResponse>>
    {

    }
}