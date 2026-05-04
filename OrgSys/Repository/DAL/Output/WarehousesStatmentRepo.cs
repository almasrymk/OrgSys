using System;
using System.IO;
using System.Linq;
using Entity.ModelReport;
using Microsoft.EntityFrameworkCore;

namespace Repository.DAL.Output
{
    public class WarehousesStatmentRepo
    {
        public OrgContext db;
        string _Schema;
        public WarehousesStatmentRepo(string Schema)
        {
            _Schema = Schema;
            if (this.db == null)
                this.db = new OrgContext(new DbContextOptions<OrgContext>() , _Schema);
        }

        public IQueryable<StockStatment> GetStocksStatment(DateTime FromDate, DateTime ToDate,
            long StockId, long ProductId, long ShiftId, long BranchId, long UserId
            )
        {
            string SQLStatment = string.Format(File.ReadAllText(Path.GetFullPath(@"SQLFiles/StockStatmentSql.sql")).Replace("Org", _Schema), string.Format("{0:yyyy/MM/dd}", FromDate), string.Format("{0:yyyy/MM/dd}", ToDate), StockId, ProductId, ShiftId, BranchId, UserId);
            return db.StockStatmentReport.FromSqlRaw(SQLStatment);
        }

        public IQueryable<ProductStatment> GetProductsStatment(DateTime FromDate, DateTime ToDate,
          long StockId, long ProductId , long ShiftId, long BranchId, long UserId
          )
        {
            string SQLStatment = string.Format(File.ReadAllText(Path.GetFullPath(@"SQLFiles/ProductStatmentSql.sql")).Replace("Org", _Schema), string.Format("{0:yyyy/MM/dd}", FromDate), string.Format("{0:yyyy/MM/dd}", ToDate), StockId, ProductId, ShiftId, BranchId, UserId);
            return db.ProductStatmentReport.FromSqlRaw(SQLStatment);
        }
    }
}