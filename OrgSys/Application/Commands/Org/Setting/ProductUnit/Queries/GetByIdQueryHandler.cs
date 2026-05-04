namespace Application.Commands.Org.Setting.ProductUnit.Queries
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

    public sealed record GetByIdProductUnitQuery(long Id) : ICommand<ProductUnitModelView> , IGetByIdQuery<Result<ProductUnitModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Entity.Model.ProductUnit> _Repository, IMapper mapper) : GetCommandHandler<GetByIdProductUnitQuery, Entity.Model.ProductUnit, ProductUnitModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.ProductUnit, bool>> CreateFilter(GetByIdProductUnitQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Utility.Status.Deleted && e.Hide != true;
        }
    }
}