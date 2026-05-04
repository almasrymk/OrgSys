namespace Application.Commands.Org.Setting.BankBranch.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record GetByIdBankBranchQuery(long Id) : ICommand<BankBranchModelView> , IGetByIdQuery<Result<BankBranchModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Entity.Model.BankBranch> _Repository, IMapper mapper) : GetCommandHandler<GetByIdBankBranchQuery, Entity.Model.BankBranch, BankBranchModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.BankBranch, bool>> CreateFilter(GetByIdBankBranchQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Utility.Status.Deleted && e.Hide != true;
        }
    }
}