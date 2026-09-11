namespace Inventory.Application.TransactionTypes.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System;
    using System.Linq.Expressions;

    public sealed record GetMaxTransactionTypeQuery(long TypeId , long ParentId) : ICommandOb<object> , IGetMaxQuery<object>;

    public sealed class GetMaxQueryHandler(IRepository<Inventory.Domain.TransactionType> _Repository) : GetMaxCommandHandler<GetMaxTransactionTypeQuery, Inventory.Domain.TransactionType>(_Repository)
    {
        public override Expression<Func<TransactionType, object>> CreateSelector()
        {
            return e => e.Id;
        }
    }
}