using System;
using Entity;
using Repository;
using System.Linq;
using X.PagedList;
using Entity.ModelView;
using Entity.ModelReport;
using System.Collections.Generic;

namespace Service
{
    public class InvoiceReportService
    {
        public InvoiceRepo repo;
        public UnitOfWorkOrg repoAll;

        public InvoiceReportService(string Schema)
        {
            repo = new InvoiceRepo(Schema);
            repoAll = new UnitOfWorkOrg(Schema);
        }

        public List<InvoiceModelView> GetInvoiceReport(InvoiceSearchModelView KeySearch)
        {
            return repo.GetList(e =>
            (e.Date >= KeySearch.FromDate.Value || KeySearch.FromDate == null) &&
            (e.Date <= KeySearch.ToDate.Value || KeySearch.ToDate == null) &&
            (e.TypeId == KeySearch.TypeId || KeySearch.TypeId == 0) &&
            (e.BranchId == KeySearch.BranchId || KeySearch.BranchId == 0) &&
            (e.ShiftId == KeySearch.ShiftId || KeySearch.ShiftId == 0) &&
            (e.CreateUserId == KeySearch.UserId || KeySearch.UserId == 0) &&
            (KeySearch.PaidStatus == 0 || (KeySearch.PaidStatus == 1 && e.Credit == 0) || (KeySearch.PaidStatus == 2 && e.Credit > 0))
            ).Select(e => e.Map<InvoiceModelView>()).ToList();
        }

        public IPagedList<InvoiceModelView> GetInvoiceReport(InvoiceSearchModelView KeySearch, int Page = 1, int PageSize = 100)
        {
            return repo.GetList(e =>
            (e.Date >= KeySearch.FromDate.Value || KeySearch.FromDate == null) &&
            (e.Date <= KeySearch.ToDate.Value || KeySearch.ToDate == null) &&
            (e.TypeId == KeySearch.TypeId || KeySearch.TypeId == 0) &&
            (e.BranchId == KeySearch.BranchId || KeySearch.BranchId == 0) &&
            (e.ShiftId == KeySearch.ShiftId || KeySearch.ShiftId == 0) &&
            (e.CreateUserId == KeySearch.UserId || KeySearch.UserId == 0) &&
            (KeySearch.PaidStatus == 0 || (KeySearch.PaidStatus == 1 && e.Credit == 0) || (KeySearch.PaidStatus == 2 && e.Credit > 0))
            ).Select(e => e.Map<InvoiceModelView>()).ToPagedList(Page, PageSize);
        }

        public List<InvoiceSumReportModelView> GetInvoiceSumReport(InvoiceSearchModelView KeySearch)
        {
            return repo.GetList(e => (e.TypeId == KeySearch.In || e.TypeId == KeySearch.Out) &&
            (e.Date >= KeySearch.FromDate.Value || KeySearch.FromDate == null) &&
            (e.Date <= KeySearch.ToDate.Value || KeySearch.ToDate == null) &&
            (e.DealerId == KeySearch.DealerId || KeySearch.DealerId == 0) &&
            (e.BranchId == KeySearch.BranchId || KeySearch.BranchId == 0) &&
            (e.ShiftId == KeySearch.ShiftId || KeySearch.ShiftId == 0) &&
            (e.CreateUserId == KeySearch.UserId || KeySearch.UserId == 0) &&
            (KeySearch.PaidStatus == 0 || (KeySearch.PaidStatus == 1 && e.Credit == 0) || (KeySearch.PaidStatus == 2 && e.Credit > 0))
            ).GroupBy(e => e).Select(e => new
            {
                DealerId = KeySearch.OrderByDealer ? 0 : e.Key.DealerId,
                DealerName = KeySearch.OrderByDealer ? "" : e.Key.Dealer.Name,
                BranchId = KeySearch.OrderByBranch ? 0 : e.Key.BranchId,
                BranchName = KeySearch.OrderByBranch ? "" : e.Key.Branch.Name,
                UserId = KeySearch.OrderByUser ? 0 : e.Key.CreateUserId,
                UserName = KeySearch.OrderByUser ? "" : e.Key.CreateUser.Name,
                ShiftId = KeySearch.OrderByShift ? 0 : e.Key.ShiftId,
                ShiftName = KeySearch.OrderByShift ? "" : e.Key.Shift.Name,
                CurrencyId = e.Key.CurrencyId,
                CurrencyName = e.Key.Currency.Name,
                Period = KeySearch.Period == 1 ? "" + e.Key.Date : (KeySearch.Period == 2 ? "" + e.Key.Date.Month : "" + e.Key.Date.Year),
                TotalRemaining = e.Where(e => e.TypeId == KeySearch.In).Sum(x => x.Remaining),
                TotalCredit = e.Where(e => e.TypeId == KeySearch.In).Sum(x => x.Credit),
                TotalInvoice = e.Where(e => e.TypeId == KeySearch.In).Sum(x => x.Net),
                TotalReturnInvoice = e.Where(e => e.TypeId == KeySearch.Out).Sum(x => x.Net),
                NetInvoice = e.Where(e => e.TypeId == KeySearch.In).Sum(x => x.Net) - e.Where(e => e.TypeId == KeySearch.Out).Sum(x => x.Net)
            }).Select(e => new InvoiceSumReportModelView
            {
                DealerId = e.DealerId,
                DealerName = e.DealerName,
                BranchId = e.BranchId ?? 0,
                BranchName = e.BranchName,
                UserId = e.UserId,
                UserName = e.UserName,
                ShiftId = e.ShiftId ?? 0,
                ShiftName = e.ShiftName,
                CurrencyId = e.CurrencyId,
                CurrencyName = e.CurrencyName,
                TotalRemaining = e.TotalRemaining,
                TotalCredit = e.TotalCredit,
                TotalInvoice = e.TotalInvoice,
                TotalReturnInvoice = e.TotalReturnInvoice,
                NetInvoice = e.NetInvoice
            }).ToList();
        }

        public IPagedList<InvoiceSumReportModelView> GetInvoiceSumReport(InvoiceSearchModelView KeySearch, int Page = 1, int PageSize = 100)
        {
            return repo.GetList(e => (e.TypeId == KeySearch.In || e.TypeId == KeySearch.Out) &&
            (e.Date >= KeySearch.FromDate.Value || KeySearch.FromDate == null) &&
            (e.Date <= KeySearch.ToDate.Value || KeySearch.ToDate == null) &&
            (e.DealerId == KeySearch.DealerId || KeySearch.DealerId == 0) &&
            (e.BranchId == KeySearch.BranchId || KeySearch.BranchId == 0) &&
            (e.ShiftId == KeySearch.ShiftId || KeySearch.ShiftId == 0) &&
            (e.CreateUserId == KeySearch.UserId || KeySearch.UserId == 0) &&
            (KeySearch.PaidStatus == 0 || (KeySearch.PaidStatus == 1 && e.Credit == 0) || (KeySearch.PaidStatus == 2 && e.Credit > 0))
            ).GroupBy(e => e).Select(e => new
            {
                DealerId = KeySearch.OrderByDealer ? 0 : e.Key.DealerId,
                DealerName = KeySearch.OrderByDealer ? "" : e.Key.Dealer.Name,
                BranchId = KeySearch.OrderByBranch ? 0 : e.Key.BranchId,
                BranchName = KeySearch.OrderByBranch ? "" : e.Key.Branch.Name,
                UserId = KeySearch.OrderByUser ? 0 : e.Key.CreateUserId,
                UserName = KeySearch.OrderByUser ? "" : e.Key.CreateUser.Name,
                ShiftId = KeySearch.OrderByShift ? 0 : e.Key.ShiftId,
                ShiftName = KeySearch.OrderByShift ? "" : e.Key.Shift.Name,
                CurrencyId = e.Key.CurrencyId,
                CurrencyName = e.Key.Currency.Name,
                Period = KeySearch.Period == 1 ? "" + e.Key.Date : (KeySearch.Period == 2 ? "" + e.Key.Date.Month : "" + e.Key.Date.Year),
                TotalRemaining = e.Where(e => e.TypeId == KeySearch.In).Sum(x => x.Remaining),
                TotalCredit = e.Where(e => e.TypeId == KeySearch.In).Sum(x => x.Credit),
                TotalInvoice = e.Where(e => e.TypeId == KeySearch.In).Sum(x => x.Net),
                TotalReturnInvoice = e.Where(e => e.TypeId == KeySearch.Out).Sum(x => x.Net),
                NetInvoice = e.Where(e => e.TypeId == KeySearch.In).Sum(x => x.Net) - e.Where(e => e.TypeId == KeySearch.Out).Sum(x => x.Net)
            }).Select(e => new InvoiceSumReportModelView
            {
                DealerId = e.DealerId,
                DealerName = e.DealerName,
                BranchId = e.BranchId ?? 0,
                BranchName = e.BranchName,
                UserId = e.UserId,
                UserName = e.UserName,
                ShiftId = e.ShiftId ?? 0,
                ShiftName = e.ShiftName,
                CurrencyId = e.CurrencyId,
                CurrencyName = e.CurrencyName,
                TotalRemaining = e.TotalRemaining,
                TotalCredit = e.TotalCredit,
                TotalInvoice = e.TotalInvoice,
                TotalReturnInvoice = e.TotalReturnInvoice,
                NetInvoice = e.NetInvoice
            }).ToPagedList(Page, PageSize);
        }
    }
}