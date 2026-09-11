namespace Inventory.Application.Products.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using System.Linq.Expressions;

    public sealed record GetMaxProductQuery(long TypeId, long ParentId) : ICommandOb<object>, IGetMaxQuery<object>;

    public sealed class GetMaxQueryHandler(IRepository<Inventory.Domain.Product> _Repository) : GetMaxCommandHandler<GetMaxProductQuery, Inventory.Domain.Product>(_Repository)
    {
        public override Expression<Func<Inventory.Domain.Product, bool>> CreateFilter(GetMaxProductQuery request)
        {
            return e => e.TypeId == request.TypeId;
        }

        public override Expression<Func<Inventory.Domain.Product, object>> CreateSelector()
        {
            return e => e.CodeNumber;
        }
    }
}
