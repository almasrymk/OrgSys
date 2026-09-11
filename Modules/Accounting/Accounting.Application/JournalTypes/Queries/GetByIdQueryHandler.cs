namespace Accounting.Application.JournalTypes.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdJournalTypeQuery(long Id) : ICommand<JournalTypeDto>, IGetByIdQuery<Result<JournalTypeDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Accounting.Domain.JournalType> repository, IMapper mapper)
        : GetCommandHandler<GetByIdJournalTypeQuery, Accounting.Domain.JournalType, JournalTypeDto>(repository, mapper)
    {
        public override Expression<Func<Accounting.Domain.JournalType, bool>> CreateFilter(GetByIdJournalTypeQuery request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted;
        }
    }
}
