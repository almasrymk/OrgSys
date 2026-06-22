namespace Application.Commands.Org.Setting.Property.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record DeletePropertyCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Property> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeletePropertyCommand, Domain.Entities.Property>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Domain.Entities.Property, bool>> CreateFilter(DeletePropertyCommand request)
        {
            return e => e.Id == request.Id && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}