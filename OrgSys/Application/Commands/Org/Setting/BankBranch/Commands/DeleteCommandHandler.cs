namespace Application.Commands.Org.Setting.BankBranch.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record DeleteBankBranchCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.BankBranch> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteBankBranchCommand, Entity.Model.BankBranch>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Entity.Model.BankBranch, bool>> CreateFilter(DeleteBankBranchCommand request)
        {
            return e => e.Id == request.Id && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}