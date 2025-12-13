namespace Application.Commands.Org.City.Queries
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

    public sealed record GetByIdQuery(long Id) : ICommand<CityModelView> , IGetByIdQuery<Result<CityModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Entity.Model.City> _Repository, IMapper mapper) : GetCommandHandler<GetByIdQuery, Entity.Model.City, CityModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.City, bool>> CreateFilter(GetByIdQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Utility.Status.Deleted && e.Hide != true;
        }
    }
}