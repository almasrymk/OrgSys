namespace Treasury.Application.Outlays.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdOutlayQuery(long Id) : ICommand<OutlayDto> , IGetByIdQuery<Result<OutlayDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Treasury.Domain.Outlay> _Repository, IMapper mapper) : GetCommandHandler<GetByIdOutlayQuery, Treasury.Domain.Outlay, OutlayDto>(_Repository, mapper)
    {
        public override Expression<Func<Treasury.Domain.Outlay, bool>> CreateFilter(GetByIdOutlayQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}