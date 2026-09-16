namespace Reporting.Infrastructure;

using OrgSys.SharedKernel;
using Catalog.Domain;
using CommercialDocuments.Domain;
using Inventory.Domain;
using MasterData.Domain;
using OrgSys.SharedKernel;
using Parties.Domain;
using Reporting.Application;
using Reporting.Application.Dealer.Queries;
using Reporting.Application.Financial.Queries;
using Reporting.Application.Sales.Queries;
using Reporting.Application.Warehouse.Queries;
using System.Net;
using Treasury.Domain;

public sealed class ReportingReadStore(
    IRepository<Invoice> invoiceRepository,
    IRepository<InvoiceType> invoiceTypeRepository,
    IRepository<Dealer> dealerRepository,
    IRepository<Financial> financialRepository,
    IRepository<FinancialType> financialTypeRepository,
    IRepository<CashBox> cashBoxRepository,
    IRepository<TransactionProduct> transactionProductRepository,
    IRepository<global::Inventory.Domain.TransactionType> transactionTypeRepository,
    IRepository<Product> productRepository,
    IRepository<Currency> currencyRepository)
    : IReportingReadStore
{
    public async Task<ResultPagination<DealerBalance>> GetDealerBalanceAsync(GetDealerBalanceReportQuery request, CancellationToken cancellationToken)
    {
        var invoiceTypes = (await invoiceTypeRepository.GetListByFilterAsync(e => true, string.Empty))?.ToList() ?? [];
        var group = request.DealerTypeId == 1 ? "Sales" : "Purchases";
        var invoiceTypeIds = invoiceTypes.Where(e => e.Group == group).Select(e => e.Id).ToHashSet();
        var inOutByInvoiceTypeId = invoiceTypes.ToDictionary(e => e.Id, e => e.InOut);

        var dealers = (await dealerRepository.GetListByFilterAsync(
            e => e.TypeId == request.DealerTypeId
                && (request.DealerId == 0 || e.Id == request.DealerId)
                && e.Status != Status.Deleted && !e.Hide,
            string.Empty))?.ToList() ?? [];
        var dealerById = dealers.ToDictionary(e => e.Id);
        var dealerIds = dealerById.Keys.ToHashSet();

        var invoices = (await invoiceRepository.GetListByFilterAsync(
            e => e.Date <= request.ToDate
                && invoiceTypeIds.Contains(e.TypeId)
                && dealerIds.Contains(e.DealerId)
                && (request.ShiftId == 0 || e.ShiftId == request.ShiftId)
                && (request.BranchId == 0 || e.BranchId == request.BranchId)
                && (request.UserId == 0 || e.CreateUserId == request.UserId)
                && e.Status != Status.Deleted && !e.Hide,
            string.Empty))?.ToList() ?? [];

        var financials = (await financialRepository.GetListByFilterAsync(
            e => e.Date <= request.ToDate
                && e.DealerId.HasValue && dealerIds.Contains(e.DealerId.Value)
                && (request.ShiftId == 0 || e.ShiftId == request.ShiftId)
                && (request.BranchId == 0 || e.BranchId == request.BranchId)
                && (request.UserId == 0 || e.CreateUserId == request.UserId)
                && e.Status != Status.Deleted && !e.Hide,
            "FinancialType"))?.ToList() ?? [];

        var balances = new Dictionary<long, DealerBalance>();

        DealerBalance GetOrAdd(long dealerId)
        {
            if (!balances.TryGetValue(dealerId, out var balance))
            {
                dealerById.TryGetValue(dealerId, out var dealer);
                balance = new DealerBalance { DealerId = dealerId, DealerName = dealer?.Name, DealerImgPath = dealer?.ImgPath };
                balances[dealerId] = balance;
            }
            return balance;
        }

        foreach (var invoice in invoices)
        {
            var inOut = inOutByInvoiceTypeId.TryGetValue(invoice.TypeId, out var io) ? io : 0;
            var balance = GetOrAdd(invoice.DealerId);
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
            var balance = GetOrAdd(dealerId);
            balance.Balance -= Math.Abs(financial.AmountByDefaultCurrency * inOut);
        }

        var ordered = balances.Values.OrderBy(e => e.DealerId).ToList();
        return Paginate(ordered, request.Page, request.PageSize);
    }

    public async Task<ResultPagination<DealerStatment>> GetDealerStatementAsync(GetDealerStatementReportQuery request, CancellationToken cancellationToken)
    {
        var invoiceTypes = (await invoiceTypeRepository.GetListByFilterAsync(e => true, string.Empty))?.ToList() ?? [];
        var group = request.DealerTypeId == 1 ? "Sales" : "Purchases";
        var invoiceTypeIds = invoiceTypes.Where(e => e.Group == group).Select(e => e.Id).ToHashSet();
        var invoiceTypeById = invoiceTypes.ToDictionary(e => e.Id, e => e);

        var dealers = (await dealerRepository.GetListByFilterAsync(
            e => e.TypeId == request.DealerTypeId
                && (request.DealerId == 0 || e.Id == request.DealerId)
                && e.Status != Status.Deleted && !e.Hide,
            string.Empty))?.ToList() ?? [];
        var dealerById = dealers.ToDictionary(e => e.Id);
        var dealerIdsFilter = dealerById.Keys.ToHashSet();

        var invoices = (await invoiceRepository.GetListByFilterAsync(
            e => invoiceTypeIds.Contains(e.TypeId)
                && dealerIdsFilter.Contains(e.DealerId)
                && (request.ShiftId == 0 || e.ShiftId == request.ShiftId)
                && (request.BranchId == 0 || e.BranchId == request.BranchId)
                && (request.UserId == 0 || e.CreateUserId == request.UserId)
                && e.Status != Status.Deleted && !e.Hide,
            string.Empty))?.ToList() ?? [];

        var financials = (await financialRepository.GetListByFilterAsync(
            e => e.DealerId.HasValue && dealerIdsFilter.Contains(e.DealerId.Value)
                && (request.ShiftId == 0 || e.ShiftId == request.ShiftId)
                && (request.BranchId == 0 || e.BranchId == request.BranchId)
                && (request.UserId == 0 || e.CreateUserId == request.UserId)
                && e.Status != Status.Deleted && !e.Hide,
            "FinancialType"))?.ToList() ?? [];

        var invoiceRows = invoices.Select(e =>
        {
            dealerById.TryGetValue(e.DealerId, out var dealer);
            var invoiceType = invoiceTypeById.GetValueOrDefault(e.TypeId);
            return new Row(e.DealerId, dealer?.Name, dealer?.ImgPath, e.Date, 1,
                invoiceType != null ? $"{invoiceType.Group} {invoiceType.Name}" : null,
                e.Code, e.Id, e.NetByDefaultCurrency, invoiceType?.InOut ?? 0, e.NetByDefaultCurrency * (invoiceType?.InOut ?? 0));
        });

        var financialRows = financials.Where(e => e.DealerId.HasValue).Select(e =>
        {
            dealerById.TryGetValue(e.DealerId!.Value, out var dealer);
            var inOut = e.FinancialType?.InOut ?? 0;
            return new Row(e.DealerId.Value, dealer?.Name, dealer?.ImgPath, e.Date, 2,
                e.FinancialType?.Name, e.Code, e.Id, e.AmountByDefaultCurrency, -1, -Math.Abs(e.AmountByDefaultCurrency * inOut));
        });

        var allRows = invoiceRows.Concat(financialRows).ToList();

        var opening = allRows
            .Where(r => r.Date < request.FromDate)
            .GroupBy(r => new { r.DealerId, r.DealerName, r.DealerImgPath })
            .ToDictionary(g => g.Key.DealerId, g => (g.Key.DealerName, g.Key.DealerImgPath, Sum: g.Sum(r => r.SignedAmount)));

        var inRange = allRows.Where(r => r.Date >= request.FromDate && r.Date <= request.ToDate).ToList();
        var dealerIds = opening.Keys.Union(inRange.Select(r => r.DealerId)).Distinct().ToList();

        var statements = new List<DealerStatment>();
        long syntheticId = 1;
        foreach (var dealerId in dealerIds)
        {
            var (name, imgPath, openingBalance) = opening.TryGetValue(dealerId, out var o)
                ? o
                : (inRange.First(r => r.DealerId == dealerId).DealerName, inRange.First(r => r.DealerId == dealerId).DealerImgPath, 0m);

            var runningBalance = openingBalance;
            statements.Add(new DealerStatment
            {
                Id = syntheticId++,
                ReferenceId = null,
                Code = null,
                OpenningBalance = 1,
                Type = 0,
                TypeName = "Opening Balance",
                Date = request.FromDate,
                DealerId = dealerId,
                DealerName = name,
                DealerImgPath = imgPath,
                Amount = openingBalance,
                InOut = 1,
                Balance = runningBalance,
            });

            foreach (var row in inRange.Where(r => r.DealerId == dealerId).OrderBy(r => r.Date))
            {
                runningBalance += row.SignedAmount;
                statements.Add(new DealerStatment
                {
                    Id = syntheticId++,
                    ReferenceId = row.ReferenceId,
                    Code = row.Code,
                    OpenningBalance = 0,
                    Type = row.Type,
                    TypeName = row.TypeName,
                    Date = row.Date,
                    DealerId = dealerId,
                    DealerName = row.DealerName,
                    DealerImgPath = row.DealerImgPath,
                    Amount = row.DisplayAmount,
                    InOut = row.DisplayInOut,
                    Balance = runningBalance,
                });
            }
        }

        var ordered = statements.OrderBy(e => e.DealerId).ThenByDescending(e => e.OpenningBalance).ThenBy(e => e.Date).ToList();
        return Paginate(ordered, request.Page, request.PageSize);
    }

    public async Task<ResultPagination<SalesBalance>> GetSalesBalanceAsync(GetSalesBalanceReportQuery request, CancellationToken cancellationToken)
    {
        var invoices = (await invoiceRepository.GetListByFilterAsync(
            e => e.Date >= request.FromDate && e.Date < request.ToDate
                && (e.TypeId == 1 || e.TypeId == 3)
                && e.Status != Status.Deleted && !e.Hide,
            string.Empty))?.ToList() ?? [];

        var grouped = invoices
            .GroupBy(e => e.Date.Date)
            .Select(g => new SalesBalance
            {
                Date = g.Key,
                InAmount = g.Where(e => e.TypeId == 1).Sum(e => e.NetByDefaultCurrency),
                OutAmount = g.Where(e => e.TypeId == 3).Sum(e => e.NetByDefaultCurrency),
                Net = g.Where(e => e.TypeId == 1).Sum(e => e.NetByDefaultCurrency),
            })
            .OrderByDescending(e => e.Date)
            .ToList();

        return Paginate(grouped, request.Page, request.PageSize);
    }

    public async Task<ResultPagination<SafeBalance>> GetSafeBalanceAsync(GetSafeBalanceReportQuery request, CancellationToken cancellationToken)
    {
        var cashBoxes = (await cashBoxRepository.GetListByFilterAsync(
            e => e.FinancialAccountId != null && (request.CashBoxId == 0 || e.Id == request.CashBoxId),
            string.Empty))?.ToList() ?? [];
        var cashBoxByFinancialAccountId = cashBoxes.Where(e => e.FinancialAccountId.HasValue).ToDictionary(e => e.FinancialAccountId!.Value, e => e);

        var financialTypes = (await financialTypeRepository.GetListByFilterAsync(e => true, string.Empty))?.ToList() ?? [];
        var inOutByTypeId = financialTypes.ToDictionary(e => e.Id, e => e.InOut);

        var financials = (await financialRepository.GetListByFilterAsync(
            e => e.Date <= request.ToDate
                && e.FinancialAccountId != null
                && (request.UserId == 0 || e.CreateUserId == request.UserId)
                && (request.ShiftId == 0 || e.ShiftId == request.ShiftId)
                && e.Status != Status.Deleted && !e.Hide,
            string.Empty))?.ToList() ?? [];

        var grouped = financials
            .Where(e => cashBoxByFinancialAccountId.ContainsKey(e.FinancialAccountId!.Value))
            .GroupBy(e => e.FinancialAccountId!.Value)
            .Select(g =>
            {
                var cashBox = cashBoxByFinancialAccountId[g.Key];
                return new SafeBalance
                {
                    Id = cashBox.Id,
                    SafeId = cashBox.Id,
                    SafeName = cashBox.Name,
                    Balance = g.Sum(e => e.AmountByDefaultCurrency * (e.FinancialTypeId.HasValue && inOutByTypeId.TryGetValue(e.FinancialTypeId.Value, out var io) ? io : 0)),
                };
            })
            .OrderBy(e => e.SafeId)
            .ToList();

        return Paginate(grouped, request.Page, request.PageSize);
    }

    public async Task<ResultPagination<SafeStatment>> GetSafeMovementAsync(GetSafeMovementReportQuery request, CancellationToken cancellationToken)
    {
        var cashBoxes = (await cashBoxRepository.GetListByFilterAsync(e => e.FinancialAccountId != null, string.Empty))?.ToList() ?? [];
        var cashBoxByFinancialAccountId = cashBoxes.Where(e => e.FinancialAccountId.HasValue).ToDictionary(e => e.FinancialAccountId!.Value, e => e);
        long? financialAccountId = request.CashBoxId != 0
            ? cashBoxes.FirstOrDefault(e => e.Id == request.CashBoxId)?.FinancialAccountId
            : null;

        var financialTypes = (await financialTypeRepository.GetListByFilterAsync(e => true, string.Empty))?.ToList() ?? [];
        var typeNameById = financialTypes.ToDictionary(e => e.Id, e => e.Name);

        var financials = (await financialRepository.GetListByFilterAsync(
            e => e.Date >= request.FromDate && e.Date <= request.ToDate
                && e.FinancialAccountId != null
                && (request.DealerId == 0 || e.DealerId == request.DealerId)
                && (financialAccountId == null || e.FinancialAccountId == financialAccountId)
                && e.Status != Status.Deleted && !e.Hide,
            string.Empty))?.ToList() ?? [];

        var ordered = financials
            .Where(e => cashBoxByFinancialAccountId.ContainsKey(e.FinancialAccountId!.Value))
            .OrderByDescending(e => e.Id)
            .ThenBy(e => e.FinancialAccountId)
            .ToList();

        var pageDealers = ordered.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();
        var dealerIds = pageDealers.Where(e => e.DealerId.HasValue).Select(e => e.DealerId!.Value).Distinct().ToList();
        var dealerNames = dealerIds.Count == 0
            ? []
            : ((await dealerRepository.GetListByFilterAsync(e => dealerIds.Contains(e.Id), string.Empty)) ?? [])
                .ToDictionary(e => e.Id, e => e.Name);

        var currencyIds = pageDealers.Select(e => e.CurrencyId).Distinct().ToList();
        var currencyNames = currencyIds.Count == 0
            ? []
            : ((await currencyRepository.GetListByFilterAsync(e => currencyIds.Contains(e.Id), string.Empty)) ?? [])
                .ToDictionary(e => e.Id, e => e.Name);

        var page = pageDealers.Select(e =>
        {
            var cashBox = cashBoxByFinancialAccountId[e.FinancialAccountId!.Value];
            return new SafeStatment
            {
                Id = e.Id,
                ReferenceId = e.Id,
                Code = e.Code,
                Amount = e.AmountByDefaultCurrency,
                TypeId = e.FinancialTypeId,
                TypeName = e.FinancialTypeId.HasValue && typeNameById.TryGetValue(e.FinancialTypeId.Value, out var name) ? name : null,
                Date = e.Date,
                SafeId = cashBox.Id,
                SafeName = cashBox.Name,
                DealerId = e.DealerId ?? 0,
                DealerName = e.DealerId is long did ? dealerNames.GetValueOrDefault(did) : null,
                CurrencyId = e.CurrencyId,
                CurrencyName = currencyNames.GetValueOrDefault(e.CurrencyId),
            };
        }).ToList();

        var pageCount = request.PageSize > 0 ? (int)Math.Ceiling(ordered.Count / (double)request.PageSize) : 0;
        return new ResultPagination<SafeStatment>(HttpStatusCode.OK, page, request.Page, request.PageSize, pageCount, null);
    }

    public async Task<ResultPagination<StockBalance>> GetWarehouseBalanceAsync(GetBalanceReportQuery request, CancellationToken cancellationToken)
    {
        var types = (await transactionTypeRepository.GetListByFilterAsync(e => true, string.Empty))?.ToList() ?? [];
        var inOutByTypeId = types.ToDictionary(e => e.Id, e => e.InOut);

        var items = (await transactionProductRepository.GetListByFilterAsync(
            e => e.Transaction!.Date <= request.ToDate
                && (request.ProductId == 0 || e.ProductId == request.ProductId)
                && (request.StockId == 0 || e.StockId == request.StockId)
                && e.Transaction.Status != Status.Deleted && !e.Transaction.Hide,
            "Transaction,Stock"))?.ToList() ?? [];

        var productIds = items.Select(e => e.ProductId).Distinct().ToList();
        var products = productIds.Count == 0
            ? []
            : ((await productRepository.GetListByFilterAsync(e => productIds.Contains(e.Id), "Classification")) ?? [])
                .ToDictionary(e => e.Id);

        if (request.ClassificationId != 0)
            items = items.Where(e => products.TryGetValue(e.ProductId, out var product) && product.ClassificationId == request.ClassificationId).ToList();

        var grouped = items
            .GroupBy(e => new
            {
                StockId = e.StockId ?? 0,
                StockName = e.Stock?.Name,
                ProductId = e.ProductId,
            })
            .Select(g =>
            {
                products.TryGetValue(g.Key.ProductId, out var product);
                return new StockBalance
                {
                    StockId = g.Key.StockId,
                    StockName = g.Key.StockName,
                    ProductId = g.Key.ProductId,
                    ProductName = product?.Name,
                    ClassificationId = product?.ClassificationId ?? 0,
                    ClassificationName = product?.Classification?.Name,
                    Balance = g.Sum(e => e.Quantity * (e.Transaction != null && inOutByTypeId.TryGetValue(e.Transaction.TypeId, out var inOut) ? inOut : 0)),
                };
            })
            .ToList();

        var ordered = request.SortByStock
            ? grouped.OrderBy(e => e.StockId).ThenBy(e => e.ProductId).ToList()
            : grouped.OrderBy(e => e.ProductId).ThenBy(e => e.StockId).ToList();

        return Paginate(ordered, request.Page, request.PageSize);
    }

    public async Task<ResultPagination<ProductStatment>> GetWarehouseMovementAsync(GetMovementReportQuery request, CancellationToken cancellationToken)
    {
        var types = (await transactionTypeRepository.GetListByFilterAsync(e => true, string.Empty))?.ToList() ?? [];
        var typeNames = types.ToDictionary(e => e.Id, e => e.Name);

        var items = (await transactionProductRepository.GetListByFilterAsync(
            e => e.Transaction!.Date >= request.FromDate && e.Transaction.Date <= request.ToDate
                && (request.StockId == 0 || e.StockId == request.StockId)
                && (request.ProductId == 0 || e.ProductId == request.ProductId)
                && e.Transaction.Status != Status.Deleted && !e.Transaction.Hide,
            "Transaction,Stock"))?.ToList() ?? [];

        var ordered = items.OrderByDescending(e => e.TransactionId).ThenBy(e => e.ProductId).ToList();
        var pageItems = ordered.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();
        var productIds = pageItems.Select(e => e.ProductId).Distinct().ToList();
        var products = productIds.Count == 0
            ? []
            : ((await productRepository.GetListByFilterAsync(e => productIds.Contains(e.Id), "Classification")) ?? [])
                .ToDictionary(e => e.Id);

        var page = pageItems.Select(e =>
        {
            products.TryGetValue(e.ProductId, out var product);
            return new ProductStatment
            {
                Id = e.Id,
                ReferenceId = e.TransactionId,
                ProductCode = product?.Code,
                TransactionCode = e.Transaction?.Code,
                TypeId = e.Transaction?.TypeId,
                TypeName = e.Transaction != null && typeNames.TryGetValue(e.Transaction.TypeId, out var name) ? name : null,
                Date = e.Transaction?.Date ?? default,
                ProductId = e.ProductId,
                ProductName = product?.Name,
                StockId = e.StockId ?? 0,
                StockName = e.Stock?.Name,
                Quantity = e.Quantity,
                ProductImgPath = product?.ImgPath,
                ClassificationId = product?.ClassificationId ?? 0,
                ClassificationName = product?.Classification?.Name,
            };
        }).ToList();

        var pageCount = request.PageSize > 0 ? (int)Math.Ceiling(ordered.Count / (double)request.PageSize) : 0;
        return new ResultPagination<ProductStatment>(HttpStatusCode.OK, page, request.Page, request.PageSize, pageCount, null);
    }

    private static ResultPagination<T> Paginate<T>(List<T> ordered, int page, int pageSize)
    {
        var slice = ordered.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        var pageCount = pageSize > 0 ? (int)Math.Ceiling(ordered.Count / (double)pageSize) : 0;
        return new ResultPagination<T>(HttpStatusCode.OK, slice, page, pageSize, pageCount, null);
    }

    private sealed record Row(long DealerId, string? DealerName, string? DealerImgPath, DateTime Date, int Type, string? TypeName, string? Code, long? ReferenceId, decimal DisplayAmount, int DisplayInOut, decimal SignedAmount);
}
