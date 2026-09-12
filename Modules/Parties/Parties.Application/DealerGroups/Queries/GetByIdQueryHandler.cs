namespace Parties.Application.DealerGroups.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdDealerGroupQuery(long Id) : ICommand<DealerGroupDto> , IGetByIdQuery<Result<DealerGroupDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Parties.Domain.DealerGroup> _Repository, IMapper mapper) : GetCommandHandler<GetByIdDealerGroupQuery, Parties.Domain.DealerGroup, DealerGroupDto>(_Repository, mapper)
    {
        public override Expression<Func<Parties.Domain.DealerGroup, bool>> CreateFilter(GetByIdDealerGroupQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}