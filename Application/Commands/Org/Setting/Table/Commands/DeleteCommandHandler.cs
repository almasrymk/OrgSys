namespace Application.Commands.Org.Setting.Table.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record DeleteTableCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Table> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteTableCommand, Domain.Entities.Table>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Domain.Entities.Table, bool>> CreateFilter(DeleteTableCommand request)
        {
            return e => e.Id == request.Id && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}