namespace Treasury.Application.BankBranches.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdBankBranchQuery(long Id) : ICommand<BankBranchDto> , IGetByIdQuery<Result<BankBranchDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Treasury.Domain.BankBranch> _Repository, IMapper mapper) : GetCommandHandler<GetByIdBankBranchQuery, Treasury.Domain.BankBranch, BankBranchDto>(_Repository, mapper)
    {
        public override Expression<Func<Treasury.Domain.BankBranch, bool>> CreateFilter(GetByIdBankBranchQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}