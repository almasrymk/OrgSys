namespace Application.Commands.Org.Setting.Branch.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record DeleteBranchCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Branch> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteBranchCommand, Domain.Entities.Branch>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Domain.Entities.Branch, bool>> CreateFilter(DeleteBranchCommand request)
        {
            return e => e.Id == request.Id && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}