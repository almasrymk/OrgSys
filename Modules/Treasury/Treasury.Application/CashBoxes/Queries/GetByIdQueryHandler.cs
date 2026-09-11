namespace Treasury.Application.CashBoxes.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdCashBoxQuery(long Id) : ICommand<CashBoxDto>, IGetByIdQuery<Result<CashBoxDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Treasury.Domain.CashBox> _Repository, IMapper mapper) : GetCommandHandler<GetByIdCashBoxQuery, Treasury.Domain.CashBox, CashBoxDto>(_Repository, mapper)
    {
        public override Expression<Func<Treasury.Domain.CashBox, bool>> CreateFilter(GetByIdCashBoxQuery request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
