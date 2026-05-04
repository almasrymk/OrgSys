namespace Application.Commands.Org.Setting.City.Queries
{
    using AutoMapper;
    using Domain.Shared;
    using Entity.ModelView;
    using Domain.Abstraction;
    using System.Linq.Expressions;
    using Application.Interfaces.CQRS;
    using Application.Common.Commands;
    using Application.Abstraction.Command;

    public sealed record GetByIdCityQuery(long Id) : ICommand<CityModelView> , IGetByIdQuery<Result<CityModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Entity.Model.City> _Repository, IMapper mapper) : GetCommandHandler<GetByIdCityQuery, Entity.Model.City, CityModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.City, bool>> CreateFilter(GetByIdCityQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Utility.Status.Deleted && e.Hide != true;
        }
    }
}