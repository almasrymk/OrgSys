namespace Application.Commands.Org.Setting.Product.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using Domain.Abstraction;
    using System.Linq.Expressions;

    public sealed record GetMaxProductQuery(long TypeId, long ParentId) : ICommandOb<object>, IGetMaxQuery<object>;

    public sealed class GetMaxQueryHandler(IRepository<Domain.Entities.Product> _Repository) : GetMaxCommandHandler<GetMaxProductQuery, Domain.Entities.Product>(_Repository)
    {
        public override Expression<Func<Domain.Entities.Product, bool>> CreateFilter(GetMaxProductQuery request)
        {
            return e => e.TypeId == request.TypeId;
        }

        public override Expression<Func<Domain.Entities.Product, object>> CreateSelector()
        {
            return e => e.CodeNumber;
        }
    }
}
