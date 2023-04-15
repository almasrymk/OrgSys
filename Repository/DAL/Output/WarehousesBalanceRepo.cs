using System;
using System.IO;
using System.Linq;
using Entity.ModelReport;
using Microsoft.EntityFrameworkCore;

namespace Repository.DAL.Output
{
    public class WarehousesBalanceRepo
    {
        public OrgContext db;
        string _Schema;
        public WarehousesBalanceRepo(string Schema)
        {
            _Schema = Schema;
            if (this.db == null)
                this.db = new OrgContext(new DbContextOptions<OrgContext>() , _Schema);
        }

        public IQueryable<StockBalance> GetStocksBalance( DateTime ToDate,
            long StockId, long ShiftId, long BranchId, long UserId
            )
        {
            string SQLStatment = string.Format(File.ReadAllText(Path.GetFullPath(@"SQLFiles/StockBalanceSql.sql")).Replace("Org", _Schema), string.Format("{0:yyyy/MM/dd}", ToDate), StockId, ShiftId, BranchId, UserId);
            return db.StockBalanceReport.FromSqlRaw(SQLStatment);
        } 
        
        public IQueryable<ProductBalance> GetProductsBalance(DateTime ToDate,
            long ProductId, long ShiftId, long BranchId, long UserId
            )
        {
            string SQLStatment = string.Format(File.ReadAllText(Path.GetFullPath(@"SQLFiles/ProductBalanceSql.sql")).Replace("Org", _Schema), string.Format("{0:yyyy/MM/dd}", ToDate), ProductId, ShiftId, BranchId, UserId);
            return db.ProductBalanceReport.FromSqlRaw(SQLStatment);
        }
    }
}