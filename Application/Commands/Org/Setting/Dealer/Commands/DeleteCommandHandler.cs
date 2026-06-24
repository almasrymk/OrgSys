namespace Application.Commands.Org.Setting.Dealer.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record DeleteDealerCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Dealer> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteDealerCommand, Domain.Entities.Dealer>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Domain.Entities.Dealer, bool>> CreateFilter(DeleteDealerCommand request)
        {
            return e => e.Id == request.Id && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}