namespace Application.Commands.Org.Financials.Receivable.Queries
{
    using Application.DTOs;
    using Domain.Abstraction;
    using Domain.Entities;
    using Domain.Enums;
    using Domain.Shared;
    using MediatR;
    using System.Net;

    /// <summary>Lists a customer's posted receipts (<c>Financial</c> rows, FinancialTypeId = Receipt),
    /// newest first — the read side of the AR Customer Receipt screen.</summary>
    public sealed record GetCustomerReceiptsQuery(long DealerId) : IRequest<ResultCollection<FinancialDto>>;

    public sealed class GetCustomerReceiptsQueryHandler(IRepository<Financial> _Repository)
        : IRequestHandler<GetCustomerReceiptsQuery, ResultCollection<FinancialDto>>
    {
        private const long ReceiptFinancialTypeId = 2;

        public async Task<ResultCollection<FinancialDto>> Handle(GetCustomerReceiptsQuery request, CancellationToken cancellationToken)
        {
            // The repository's (Filter, orderBy) overload does not apply the orderBy argument
            // (see Infrastructure/Persistence/UnitOfWork/Repository.cs) — sort client-side instead.
            var rows = await _Repository.GetListByFilterAsync(
                e => e.DealerId == request.DealerId
                    && e.FinancialTypeId == ReceiptFinancialTypeId
                    && e.Status != Status.Deleted);

            var result = (rows ?? []).OrderByDescending(e => e.Id).Select(e => new FinancialDto
            {
                Id = e.Id,
                Code = e.Code,
                Date = e.Date,
                DealerId = e.DealerId,
                FinancialAccountId = e.FinancialAccountId,
                Amount = e.Amount,
                CurrencyId = e.CurrencyId,
                Notes = e.Notes,
                Status = e.Status,
                Posted = e.Posted,
                JournalId = e.JournalId
            }).ToList();

            return new ResultCollection<FinancialDto>(HttpStatusCode.OK, result, null);
        }
    }
}
