using System;
using System.IO;
using System.Linq;
using Application.Report;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Repository.DAL.Output
{
    public class DealersBalanceRepo
    {
        public OrgContext db;
        string _Schema;
        public DealersBalanceRepo(string Schema)
        {
            _Schema = Schema;
            if (this.db == null)
                this.db = new OrgContext(new DbContextOptions<OrgContext>() , _Schema);
        }

        public IQueryable<DealerBalance> GetDealersBalance(int DealerTypeId , DateTime ToDate,
            long DealerId, long ShiftId, long BranchId, long UserId
            )
        {
            string SQLStatment = string.Format(File.ReadAllText(Path.GetFullPath(@"SQLFiles/DealersBalanceSql.sql")).Replace("Org", _Schema), DealerTypeId, string.Format("{0:yyyy/MM/dd}", ToDate), DealerId, ShiftId, BranchId, UserId);
            return db.DealerBalanceReport.FromSqlRaw(SQLStatment);
        } 
        
        public IQueryable<SalesBalance> GetSalesBalance( DateTime Date,
             long UserId
            )
        {
            string SQLStatment = string.Format(File.ReadAllText(Path.GetFullPath(@"SQLFiles/SalesBalanceSql.sql")).Replace("Org", _Schema), string.Format("{0:yyyy/MM/dd}", Date), UserId);
            return db.SalesBalanceReport.FromSqlRaw(SQLStatment);
        }  
        
        public IQueryable<SalesClient> GetSalesClient(DateTime Date, long DealerId,
             long UserId
            )
        {
            string SQLStatment = string.Format(File.ReadAllText(Path.GetFullPath(@"SQLFiles/SalesClientSql.sql")).Replace("Org", _Schema), string.Format("{0:yyyy/MM/dd}", Date), DealerId, UserId);
            return db.SalesClientReport.FromSqlRaw(SQLStatment);
        }
    }
}