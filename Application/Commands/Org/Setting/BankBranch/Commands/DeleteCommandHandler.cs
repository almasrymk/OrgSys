namespace Application.Commands.Org.Setting.BankBranch.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record DeleteBankBranchCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.BankBranch> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteBankBranchCommand, Domain.Entities.BankBranch>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Domain.Entities.BankBranch, bool>> CreateFilter(DeleteBankBranchCommand request)
        {
            return e => e.Id == request.Id && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}