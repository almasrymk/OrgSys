namespace Organization.Application.Shifts.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdShiftQuery(long Id) : ICommand<ShiftDto> , IGetByIdQuery<Result<ShiftDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Organization.Domain.Shift> _Repository, IMapper mapper) : GetCommandHandler<GetByIdShiftQuery, Organization.Domain.Shift, ShiftDto>(_Repository, mapper)
    {
        public override Expression<Func<Organization.Domain.Shift, bool>> CreateFilter(GetByIdShiftQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}