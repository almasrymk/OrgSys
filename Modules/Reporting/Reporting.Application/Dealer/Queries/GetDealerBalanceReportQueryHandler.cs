namespace Reporting.Application.Dealer.Queries
{
    using OrgSys.SharedKernel;
    using Treasury.Domain;
    using System.Net;

    /// <summary>
    /// Backs both "Suppliers Balance" (DealerTypeId=2) and "Clients Balance" (DealerTypeId=1) —
    /// MVC's PurchasesController.SuppliersBalance / SalesController.ClientsBalance, both calling the
    /// legacy SalesReportService.GetDealersBalance(dealerTypeId, ...). Balance-as-of-`ToDate`: Invoice
    /// leg signed by InvoiceType.InOut (filtered to the dealer type's Sales/Purchases InvoiceType
    /// group), Financial leg always subtracted (payments/receipts always reduce the invoice-driven
    /// balance in this model, matching the legacy SQL).
    /// </summary>
    public sealed record GetDealerBalanceReportQuery(
        long DealerTypeId, DateTime ToDate, long DealerId, long ShiftId, long BranchId, long UserId, int Page, int PageSize)
        : ICommandPagination<DealerBalance>;

    public sealed class GetDealerBalanceReportQueryHandler(
        IRepository<Invoice> invoiceRepository,
        IRepository<Financial> financialRepository,
        IRepository<global::CommercialDocuments.Domain.InvoiceType> invoiceTypeRepository)
        : ICommandPaginationHandler<GetDealerBalanceReportQuery, DealerBalance>
    {
        public async Task<ResultPagination<DealerBalance>> Handle(GetDealerBalanceReportQuery request, CancellationToken cancellationToken)
        {
            var invoiceTypes = (await invoiceTypeRepository.GetListByFilterAsync(e => true, string.Empty))?.ToList() ?? [];
            var group = request.DealerTypeId == 1 ? "Sales" : "Purchases";
            var invoiceTypeIds = invoiceTypes.Where(e => e.Group == group).Select(e => e.Id).ToHashSet();
            var inOutByInvoiceTypeId = invoiceTypes.ToDictionary(e => e.Id, e => e.InOut);

            var invoices = (await invoiceRepository.GetListByFilterAsync(
                e => e.Date <= request.ToDate
                    && invoiceTypeIds.Contains(e.TypeId)
                    && e.Dealer!.TypeId == request.DealerTypeId
                    && (request.DealerId == 0 || e.DealerId == request.DealerId)
                    && (request.ShiftId == 0 || e.ShiftId == request.ShiftId)
                    && (request.BranchId == 0 || e.BranchId == request.BranchId)
                    && (request.UserId == 0 || e.CreateUserId == request.UserId)
                    && e.Status != Status.Deleted && !e.Hide,
                "Dealer"))?.ToList() ?? [];

            var financials = (await financialRepository.GetListByFilterAsync(
                e => e.Date <= request.ToDate
                    && e.Dealer != null && e.Dealer.TypeId == request.DealerTypeId
                    && (request.DealerId == 0 || e.DealerId == request.DealerId)
                    && (request.ShiftId == 0 || e.ShiftId == request.ShiftId)
                    && (request.BranchId == 0 || e.BranchId == request.BranchId)
                    && (request.UserId == 0 || e.CreateUserId == request.UserId)
                    && e.Status != Status.Deleted && !e.Hide,
                "Dealer,FinancialType"))?.ToList() ?? [];

            var balances = new Dictionary<long, DealerBalance>();

            DealerBalance GetOrAdd(long dealerId, string? name, string? imgPath)
            {
                if (!balances.TryGetValue(dealerId, out var balance))
                {
                    balance = new DealerBalance { DealerId = dealerId, DealerName = name, DealerImgPath = imgPath };
                    balances[dealerId] = balance;
                }
                return balance;
            }

            foreach (var invoice in invoices)
            {
                var inOut = inOutByInvoiceTypeId.TryGetValue(invoice.TypeId, out var io) ? io : 0;
                var balance = GetOrAdd(invoice.DealerId, invoice.Dealer?.Name, invoice.Dealer?.ImgPath);
                balance.Balance += invoice.NetByDefaultCurrency * inOut;
                if (inOut > 0) balance.TotalInvoice += invoice.NetByDefaultCurrency;
                else if (inOut < 0) balance.TotalReturnInvoice += invoice.NetByDefaultCurrency;
                balance.TotalCreditInvoice += invoice.CreditByDefaultCurrency * inOut;
                balance.TotalPaidInvoice += invoice.Paid * invoice.Rate * inOut;
            }

            foreach (var financial in financials)
            {
                if (financial.DealerId is not long dealerId) continue;
                var inOut = financial.FinancialType?.InOut ?? 0;
                var balance = GetOrAdd(dealerId, financial.Dealer?.Name, financial.Dealer?.ImgPath);
                balance.Balance -= Math.Abs(financial.AmountByDefaultCurrency * inOut);
            }

            var ordered = balances.Values.OrderBy(e => e.DealerId).ToList();
            var page = ordered.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();
            var pageCount = request.PageSize > 0 ? (int)Math.Ceiling(ordered.Count / (double)request.PageSize) : 0;
            return new ResultPagination<DealerBalance>(HttpStatusCode.OK, page, request.Page, request.PageSize, pageCount, null);
        }
    }
}
