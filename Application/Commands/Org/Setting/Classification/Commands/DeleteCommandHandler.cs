namespace Application.Commands.Org.Setting.Classification.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record DeleteClassificationCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Classification> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteClassificationCommand, Domain.Entities.Classification>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Domain.Entities.Classification, bool>> CreateFilter(DeleteClassificationCommand request)
        {
            return e => e.Id == request.Id && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}