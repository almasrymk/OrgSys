using System;
using Repository;
using X.PagedList;
using System.Linq;
using Entity.ModelReport;
using Utility;

namespace Service
{
    public class SalesReportService
    {
        UnitOfWorkOrg repo;
        private string _Schema;
        public SalesReportService(string Schema)
        {
            this._Schema = Schema;
            repo = new UnitOfWorkOrg(Schema);
        }

        public IPagedList<DealerStatment> GetDealersStatment(
          long DealerTypeId, DateTime FromDate, DateTime ToDate,
          long DealerId, long ShiftId, long BranchId, long UserId,
          int page = 1, int pageSize = 100)
        {
            return repo.dealersStatmentRepo.GetDealersStatment(DealerTypeId, FromDate, ToDate, DealerId, ShiftId, BranchId, UserId).OrderByDescending(e => e.ReferenceId).ThenBy(e => e.DealerId).AsEnumerable().ToPagedList(page, pageSize);
        }

        public IPagedList<DealerBalance> GetDealersBalance(int DealerTypeId,
            DateTime ToDate,
            long DealerId, long ShiftId, long BranchId, long UserId,
            int page = 1, int pageSize = 100)
        {
            return repo.dealersBalanceRepo.GetDealersBalance(DealerTypeId , ToDate, DealerId, ShiftId, BranchId, UserId).OrderBy(e=>e.DealerId).AsEnumerable().ToPagedList(page, pageSize);
        } 
        public IPagedList<SalesBalance> GetSalesBalance(
            DateTime ToDate,
             long UserId,
            int page = 1, int pageSize = 100)
        {
            return repo.dealersBalanceRepo.GetSalesBalance(ToDate,  UserId).AsEnumerable().ToPagedList(page, pageSize);
        }
    }
}