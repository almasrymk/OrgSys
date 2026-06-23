namespace Application.Commands.Org.Setting.Unit.Queries
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

    public sealed record GetByIdUnitQuery(long Id) : ICommand<UnitDto> , IGetByIdQuery<Result<UnitDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.Unit> _Repository, IMapper mapper) : GetCommandHandler<GetByIdUnitQuery, Domain.Entities.Unit, UnitDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Unit, bool>> CreateFilter(GetByIdUnitQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}