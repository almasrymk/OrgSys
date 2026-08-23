namespace Application.Commands.Org.Financials.Payable.Queries
{
    using Application.DTOs;
    using Domain.Abstraction;
    using Domain.Entities;
    using Domain.Enums;
    using Domain.Shared;
    using MediatR;
    using System.Net;

    /// <summary>Lists a supplier's posted payments (<c>Financial</c> rows, FinancialTypeId = Payment),
    /// newest first — the read side of the AP Supplier Payment screen. Mirrors
    /// <c>GetCustomerReceiptsQueryHandler</c> on the AR side.</summary>
    public sealed record GetSupplierPaymentsQuery(long DealerId) : IRequest<ResultCollection<FinancialDto>>;

    public sealed class GetSupplierPaymentsQueryHandler(IRepository<Financial> _Repository)
        : IRequestHandler<GetSupplierPaymentsQuery, ResultCollection<FinancialDto>>
    {
        private const long PaymentFinancialTypeId = 3;

        public async Task<ResultCollection<FinancialDto>> Handle(GetSupplierPaymentsQuery request, CancellationToken cancellationToken)
        {
            // The repository's (Filter, orderBy) overload does not apply the orderBy argument
            // (see Infrastructure/Persistence/UnitOfWork/Repository.cs) — sort client-side instead.
            var rows = await _Repository.GetListByFilterAsync(
                e => e.DealerId == request.DealerId
                    && e.FinancialTypeId == PaymentFinancialTypeId
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
