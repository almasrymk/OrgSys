namespace Treasury.Application.Financials.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System;
    using System.Linq.Expressions;

    public sealed record GetMaxFinancialQuery(long TypeId , long ParentId) : ICommandOb<object> , IGetMaxQuery<object>;

    public sealed class GetMaxQueryHandler(IRepository<Financial> _Repository) : GetMaxCommandHandler<GetMaxFinancialQuery, Financial>(_Repository)
    {
        public override Expression<Func<Financial, bool>> CreateFilter(GetMaxFinancialQuery request)
        {
            return e=>e.TypeId == request.TypeId;
        }

        public override Expression<Func<Financial, object>> CreateSelector()
        {
            return e => e.CodeNumber;
        }
    }
}