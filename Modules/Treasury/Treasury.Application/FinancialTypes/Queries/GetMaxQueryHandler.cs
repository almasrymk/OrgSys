namespace Treasury.Application.FinancialTypes.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System;
    using System.Linq.Expressions;

    public sealed record GetMaxFinancialTypeQuery(long TypeId , long ParentId) : ICommandOb<object> , IGetMaxQuery<object>;

    public sealed class GetMaxQueryHandler(IRepository<FinancialType> _Repository) : GetMaxCommandHandler<GetMaxFinancialTypeQuery, FinancialType>(_Repository)
    {
        public override Expression<Func<FinancialType, bool>> CreateFilter(GetMaxFinancialTypeQuery request)
        {
            return e=>e.TypeId == request.TypeId;
        }

        public override Expression<Func<FinancialType, object>> CreateSelector()
        {
            return e => e.CodeNumber;
        }
    }
}