namespace Inventory.Application.Inventories.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System;
    using System.Linq.Expressions;

    public sealed record GetMaxInventoryQuery(long TypeId , long ParentId) : ICommandOb<object> , IGetMaxQuery<object>;

    public sealed class GetMaxQueryHandler(IRepository<global::Inventory.Domain.Inventory> _Repository) : GetMaxCommandHandler<GetMaxInventoryQuery, global::Inventory.Domain.Inventory>(_Repository)
    {
        public override Expression<Func<global::Inventory.Domain.Inventory, bool>> CreateFilter(GetMaxInventoryQuery request)
        {
            return e=>e.TypeId == request.TypeId;
        }

        public override Expression<Func<global::Inventory.Domain.Inventory, object>> CreateSelector()
        {
            return e => e.CodeNumber;
        }
    }
}