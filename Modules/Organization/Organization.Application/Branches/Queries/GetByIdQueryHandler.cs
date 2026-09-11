namespace Organization.Application.Branches.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdBranchQuery(long Id) : ICommand<BranchDto> , IGetByIdQuery<Result<BranchDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Organization.Domain.Branch> _Repository, IMapper mapper) : GetCommandHandler<GetByIdBranchQuery, Organization.Domain.Branch, BranchDto>(_Repository, mapper)
    {
        public override Expression<Func<Organization.Domain.Branch, bool>> CreateFilter(GetByIdBranchQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}