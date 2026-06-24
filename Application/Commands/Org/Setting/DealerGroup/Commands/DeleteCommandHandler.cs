namespace Application.Commands.Org.Setting.DealerGroup.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record DeleteDealerGroupCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.DealerGroup> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteDealerGroupCommand, Domain.Entities.DealerGroup>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Domain.Entities.DealerGroup, bool>> CreateFilter(DeleteDealerGroupCommand request)
        {
            return e => e.Id == request.Id && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}