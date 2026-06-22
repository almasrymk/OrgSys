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

    public sealed record GetByIdUnitQuery(long Id) : ICommand<UnitModelView> , IGetByIdQuery<Result<UnitModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.Unit> _Repository, IMapper mapper) : GetCommandHandler<GetByIdUnitQuery, Domain.Entities.Unit, UnitModelView>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Unit, bool>> CreateFilter(GetByIdUnitQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}