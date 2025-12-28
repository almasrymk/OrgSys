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

    public sealed record DeleteListBankBranchCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.BankBranch> _Repository , IMapper mapper) : DeleteCommandHandler<DeleteListBankBranchCommand, Entity.Model.BankBranch>(_UnitOfWork, _Repository , mapper)
    {
        public override Expression<Func<Entity.Model.BankBranch, bool>> CreateFilter(DeleteListBankBranchCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}