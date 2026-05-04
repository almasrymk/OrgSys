using System;
using System.IO;
using System.Linq;
using Entity.ModelReport;
using Microsoft.EntityFrameworkCore;

namespace Repository.DAL.Output
{
    public class SafeStatmentRepo
    {
        public OrgContext db;
        string _Schema;
        public SafeStatmentRepo(string Schema)
        {
            _Schema = Schema;
            if (this.db == null)
                this.db = new OrgContext(new DbContextOptions<OrgContext>() , _Schema);
        }
        public IQueryable<SafeStatment> GetSafeStatment(DateTime FromDate, DateTime ToDate,
          long DealerId, long SafeId, long ShiftId, long BranchId, long UserId
          )
        {
            string SQLStatment = string.Format(File.ReadAllText(Path.GetFullPath(@"SQLFiles/SafeStatmentSql.sql")).Replace("Org", _Schema), string.Format("{0:yyyy/MM/dd}", FromDate), string.Format("{0:yyyy/MM/dd}", ToDate), DealerId, SafeId, ShiftId, BranchId, UserId);
            return db.SafeStatmentReport.FromSqlRaw(SQLStatment);
        }


        public IQueryable<SafeBalance> GetSafeBalance(DateTime ToDate,
         long SafeId, long ShiftId, long BranchId, long UserId
         )
        {
            string SQLStatment = "";//string.Format(File.ReadAllText(Path.GetFullPath(@"SQLFiles/SafeBalanceSql.sql")).Replace("Org", _Schema), string.Format("{0:yyyy/MM/dd}", FromDate), string.Format("{0:yyyy/MM/dd}", ToDate), DealerId, SafeId, ShiftId, BranchId, UserId);
            return db.SafeBalanceReport.FromSqlRaw(SQLStatment);
        }
    }
}