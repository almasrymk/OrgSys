namespace Application.Commands.Org.Setting.City.Queries
{
    using AutoMapper;
    using Domain.Shared;
    using Application.DTOs;
    using Domain.Abstraction;
    using System.Linq.Expressions;
    using Application.Interfaces.CQRS;
    using Application.Common.Commands;
    using Application.Abstraction.Command;

    public sealed record GetByIdCityQuery(long Id) : ICommand<CityDto> , IGetByIdQuery<Result<CityDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.City> _Repository, IMapper mapper) : GetCommandHandler<GetByIdCityQuery, Domain.Entities.City, CityDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.City, bool>> CreateFilter(GetByIdCityQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}