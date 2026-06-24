namespace Application.Commands.Org.Setting.District.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record GetByIdDistrictQuery(long Id) : ICommand<DistrictDto> , IGetByIdQuery<Result<DistrictDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.District> _Repository, IMapper mapper) : GetCommandHandler<GetByIdDistrictQuery, Domain.Entities.District, DistrictDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.District, bool>> CreateFilter(GetByIdDistrictQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}