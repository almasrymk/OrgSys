namespace Treasury.Application.FinancialAccounts.Queries
{
    using OrgSys.SharedKernel;
    using MediatR;

    public sealed record GetFinancialAccountBalanceQuery(long FinancialAccountId, DateTime? AsOfDate = null) : IRequest<Result<decimal>>;

    public sealed class GetBalanceQueryHandler(IRepository<Financial> repository)
        : IRequestHandler<GetFinancialAccountBalanceQuery, Result<decimal>>
    {
        public async Task<Result<decimal>> Handle(GetFinancialAccountBalanceQuery request, CancellationToken cancellationToken)
        {
            var asOfDate = request.AsOfDate?.Date;
            var rows = await repository.GetListByFilterAsync(e =>
                e.FinancialAccountId == request.FinancialAccountId && e.Posted && e.Status == Status.Approved
                && (asOfDate == null || e.Date.Date <= asOfDate));
            var balance = (rows ?? []).Sum(e => e.Direction == FinancialTransactionDirection.In ? e.Amount : -e.Amount);
            return new Result<decimal>(System.Net.HttpStatusCode.OK, balance, null);
        }
    }
}
