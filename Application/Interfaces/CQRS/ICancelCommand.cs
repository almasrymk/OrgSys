using Domain.Shared;
using MediatR;

namespace Application.Interfaces.CQRS
{
    public interface ICancelCommand : IRequest<Result> {

        long Id { get; }
    }
}