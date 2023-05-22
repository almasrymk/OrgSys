using System;
using Repository;
using X.PagedList;
using System.Linq;
using Entity.ModelReport;
using Utility;
using Entity.Model;

namespace Service
{
    public class WarehousesReportService
    {
        UnitOfWorkOrg repo;
        private string _Schema;
        public WarehousesReportService(string Schema)
        {
            this._Schema = Schema;
            repo = new UnitOfWorkOrg(Schema);
        }

        public IPagedList<StockStatment> GetStockStatment(
          DateTime FromDate, DateTime ToDate,
          long StockId, long ProductId, long ShiftId, long BranchId, long UserId,
          int page = 1, int pageSize = 100)
        {
            return repo.warehousesStatmentRepo.GetStocksStatment(FromDate, ToDate, ProductId, StockId, ShiftId, BranchId, UserId).OrderByDescending(e => e.ReferenceId).ThenBy(e => e.StockId).AsEnumerable().ToPagedList(page, pageSize);
        }

        public IPagedList<ProductStatment> GetProductStatment(
        DateTime FromDate, DateTime ToDate,
        long StockId,long ProductId, long ShiftId, long BranchId, long UserId,
        int page = 1, int pageSize = 100)
        {
            return repo.warehousesStatmentRepo.GetProductsStatment(FromDate, ToDate, StockId, ProductId, ShiftId, BranchId, UserId).OrderByDescending(e => e.ReferenceId).ThenBy(e => e.ProductId).AsEnumerable().ToPagedList(page, pageSize);
        }

        public IPagedList<StockBalance> GetStocksBalance(int DealerTypeId,
          DateTime ToDate,
          long ProductId, long StockId, long ClassificationId, long ShiftId, long BranchId, long UserId,
          int page = 1, int pageSize = 100)
        {
            return repo.warehousesBalanceRepo.GetStocksBalance(ToDate, ProductId, StockId, ClassificationId, ShiftId, BranchId, UserId).OrderBy(e => e.StockId).AsEnumerable().ToPagedList(page, pageSize);
        }

        public IPagedList<ProductBalance> GetProductsBalance(int DealerTypeId,
          DateTime ToDate,
          long ProductId, long StockId, long ClassificationId, long ShiftId, long BranchId, long UserId,
          int page = 1, int pageSize = 100)
        {
            return repo.warehousesBalanceRepo.GetProductsBalance( ToDate, ProductId, StockId, ClassificationId, ShiftId, BranchId, UserId).OrderBy(e => e.ProductId).AsEnumerable().ToPagedList(page, pageSize);
        }
    }
}