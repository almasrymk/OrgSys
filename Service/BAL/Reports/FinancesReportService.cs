using System;
using Repository;
using X.PagedList;
using System.Linq;
using Domain.Entities;
using X.PagedList.Extensions;
using Application.Report;

namespace Service
{
    public class FinancesReportService
    {
        UnitOfWorkOrg repo;
        private string _Schema;
        public FinancesReportService(string Schema)
        {
            this._Schema = Schema;
            repo = new UnitOfWorkOrg(Schema);
        }

        public IPagedList<SafeStatment> GetSafeStatment(
        DateTime FromDate, DateTime ToDate,
        long DealerId, long SafeId, long ShiftId, long BranchId, long UserId,
        int page = 1, int pageSize = 100)
        {
            return repo.SafeStatmentRepo.GetSafeStatment(FromDate, ToDate, DealerId, SafeId, ShiftId, BranchId, UserId).OrderByDescending(e => e.ReferenceId).ThenBy(e => e.SafeId).AsEnumerable().ToPagedList(page, pageSize);
        }

        public IPagedList<SafeBalance> GetSafeBalance(
          DateTime ToDate,
          long SafeId, long ShiftId, long BranchId, long UserId,
          int page = 1, int pageSize = 100)
        {
            //return repo.SafeBalanceRepo.GetSafeBalance(ToDate, SafeId, ShiftId, BranchId, UserId).OrderBy(e => e.SafeId).AsEnumerable().ToPagedList(page, pageSize);
            return null;
        }
    }
}