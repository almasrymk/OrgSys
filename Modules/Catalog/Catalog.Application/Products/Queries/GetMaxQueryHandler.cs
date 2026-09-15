namespace Catalog.Application.Products.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using System.Linq.Expressions;

    public sealed record GetMaxProductQuery(long TypeId, long ParentId) : ICommandOb<object>, IGetMaxQuery<object>;

    public sealed class GetMaxQueryHandler(IRepository<Catalog.Domain.Product> _Repository) : GetMaxCommandHandler<GetMaxProductQuery, Catalog.Domain.Product>(_Repository)
    {
        public override Expression<Func<Catalog.Domain.Product, bool>> CreateFilter(GetMaxProductQuery request)
        {
            return e => e.TypeId == request.TypeId;
        }

        public override Expression<Func<Catalog.Domain.Product, object>> CreateSelector()
        {
            return e => e.CodeNumber;
        }
    }
}
