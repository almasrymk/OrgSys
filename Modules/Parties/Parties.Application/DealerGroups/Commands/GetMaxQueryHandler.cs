namespace Parties.Application.DealerGroups.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System;
    using System.Linq.Expressions;

    public sealed record GetMaxDealerGroupQuery(long TypeId , long ParentId) : ICommandOb<object> , IGetMaxQuery<object>;

    public sealed class GetMaxQueryHandler(IRepository<DealerGroup> _Repository) : GetMaxCommandHandler<GetMaxDealerGroupQuery,DealerGroup>(_Repository)
    {
        public override Expression<Func<DealerGroup, bool>> CreateFilter(GetMaxDealerGroupQuery request)
        {
            return e=>e.TypeId == request.TypeId;
        }

        public override Expression<Func<DealerGroup, object>> CreateSelector()
        {
            return e => e.CodeNumber;
        }
    }
}