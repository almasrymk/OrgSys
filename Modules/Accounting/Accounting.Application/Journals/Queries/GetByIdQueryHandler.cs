namespace Accounting.Application.Journals.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdJournalQuery(long Id) : ICommand<JournalDto> , IGetByIdQuery<Result<JournalDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Accounting.Domain.Journal> _Repository, IMapper mapper) :
        GetCommandHandler<GetByIdJournalQuery, Accounting.Domain.Journal, JournalDto>(_Repository, mapper)
    {
        public override string CreateInclude()
        {
            return "JournalItems,JournalItems.Account,FiscalYear,FiscalPeriod,OriginalJournal,ReversalJournal";
        }

        public override Expression<Func<Accounting.Domain.Journal, bool>> CreateFilter(GetByIdJournalQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}