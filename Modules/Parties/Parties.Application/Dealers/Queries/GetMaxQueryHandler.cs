namespace Parties.Application.Dealers.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System;
    using System.Linq.Expressions;

    public sealed record GetMaxDealerQuery(long TypeId , long ParentId) : ICommandOb<object> , IGetMaxQuery<object>;

    public sealed class GetMaxQueryHandler(IRepository<Dealer> _Repository) : GetMaxCommandHandler<GetMaxDealerQuery, Dealer>(_Repository)
    {
        public override Expression<Func<Dealer, bool>> CreateFilter(GetMaxDealerQuery request)
        {
            return e=>e.TypeId == request.TypeId;
        }

        public override Expression<Func<Dealer, object>> CreateSelector()
        {
            return e => e.CodeNumber;
        }
    }
}