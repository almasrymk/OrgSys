namespace Accounting.Application.Accounts.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System;
    using System.Linq.Expressions;

    public sealed record GetMaxAccountQuery(long TypeId , long ParentId) : ICommandOb<object> , IGetMaxQuery<object>;

    public sealed class GetMaxQueryHandler(IRepository<Accounting.Domain.Account> _Repository) : GetMaxCommandHandler<GetMaxAccountQuery, Accounting.Domain.Account>(_Repository)
    {
        public override Expression<Func<Accounting.Domain.Account, bool>> CreateFilter(GetMaxAccountQuery request)
        {
            return e=>e.TypeId == request.TypeId;
        }

        public override Expression<Func<Accounting.Domain.Account, object>> CreateSelector()
        {
            return e => e.CodeNumber;
        }
    }
}