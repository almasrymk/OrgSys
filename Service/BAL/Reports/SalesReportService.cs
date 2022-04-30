using System;
using Repository;
using X.PagedList;
using System.Linq;
using Entity.ModelReport;

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

        public IPagedList<DealerStatmentReport> GetDealersStatment(
            long DealerTypeId, DateTime FromDate, DateTime ToDate,
            long DealerId, long ShiftId, long BranchId, long UserId,
            int page = 1, int pageSize = 100)
        {
            return repo.dealersStatmentRepo.GetDealersStatment(DealerTypeId, FromDate, ToDate, DealerId, ShiftId, BranchId, UserId).OrderBy(e=>e.DealerId).ThenByDescending(e=>e.OpenningBalance).ThenBy(e=>e.Date).AsEnumerable().ToPagedList(page, pageSize);
        }
    }
}