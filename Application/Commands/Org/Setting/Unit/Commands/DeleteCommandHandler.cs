namespace Application.Commands.Org.Setting.Unit.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record DeleteUnitCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Unit> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteUnitCommand, Domain.Entities.Unit>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Domain.Entities.Unit, bool>> CreateFilter(DeleteUnitCommand request)
        {
            return e => e.Id == request.Id && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}