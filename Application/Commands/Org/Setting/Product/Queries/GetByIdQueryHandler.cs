namespace Application.Commands.Org.Setting.Product.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record GetByIdProductQuery(long Id) : ICommand<ProductModelView> , IGetByIdQuery<Result<ProductModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Entity.Model.Product> _Repository, IMapper mapper) : GetCommandHandler<GetByIdProductQuery, Entity.Model.Product, ProductModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Product, bool>> CreateFilter(GetByIdProductQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Utility.Status.Deleted && e.Hide != true;
        }
    }
}