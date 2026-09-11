namespace Treasury.Application.Financials.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdFinancialQuery(long Id) : ICommand<FinancialDto> , IGetByIdQuery<Result<FinancialDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Treasury.Domain.Financial> _Repository, IMapper mapper) :
        GetCommandHandler<GetByIdFinancialQuery, Treasury.Domain.Financial, FinancialDto>(_Repository, mapper)
    {
        public override string CreateInclude()
        {
            // FinancialAccount/FinancialType aren't read from these navs anywhere on the Save screen —
            // its dropdowns/names come from separate API calls — and FinancialAccount in particular
            // pulls in CashBox/BankAccount's circular back-reference to their own FinancialAccount, which
            // silently failed the query (GetCommandHandler<> swallows the exception into a null Response,
            // which this screen's edit-load then misreads as "record not found" and shows a blank Draft).
            return "FinancialInvoices,FinancialInvoices.Invoice";
        }

        public override Expression<Func<Treasury.Domain.Financial, bool>> CreateFilter(GetByIdFinancialQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}