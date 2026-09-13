namespace Accounting.Application.Journals.Queries
{
    using MasterData.Contracts.Currencies;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using MediatR;
    using System.Linq.Expressions;

    public sealed record GetByIdJournalQuery(long Id) : ICommand<JournalDto> , IGetByIdQuery<Result<JournalDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Accounting.Domain.Journal> _Repository, IMapper mapper, ISender sender) :
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

        public override async Task<Result<JournalDto>> Handle(GetByIdJournalQuery request, CancellationToken cancellationToken)
        {
            var result = await base.Handle(request, cancellationToken);
            if (result.Response is null || result.Response.Id == 0)
                return result;

            var currencyResult = await sender.Send(new GetCurrencyNamesQuery([result.Response.CurrencyId]), cancellationToken);
            result.Response.CurrencyName = (currencyResult.Response ?? []).GetValueOrDefault(result.Response.CurrencyId);

            return result;
        }
    }
}
