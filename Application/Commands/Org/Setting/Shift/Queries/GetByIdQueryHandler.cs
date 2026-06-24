namespace Application.Commands.Org.Setting.Shift.Queries
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

    public sealed record GetByIdShiftQuery(long Id) : ICommand<ShiftDto> , IGetByIdQuery<Result<ShiftDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.Shift> _Repository, IMapper mapper) : GetCommandHandler<GetByIdShiftQuery, Domain.Entities.Shift, ShiftDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Shift, bool>> CreateFilter(GetByIdShiftQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}