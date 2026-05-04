using Entity.ModelReport;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DAL.Output
{
    public class SafeBalanceRepo
    {
        public OrgContext db;
        string _Schema;
        public SafeBalanceRepo(string Schema)
        {
            _Schema = Schema;
            if (this.db == null)
                this.db = new OrgContext(new DbContextOptions<OrgContext>(), _Schema);
        }



        //public IQueryable<SafeStatment> GetSafeBalance(DateTime ToDate,
        //  long SafeId, long ShiftId, long BranchId, long UserId
        //  )
        //{
        //    string SQLStatment = string.Format(File.ReadAllText(Path.GetFullPath(@"SQLFiles/SafeStatmentSql.sql")).Replace("Org", _Schema), string.Format("{0:yyyy/MM/dd}", ToDate), SafeId, ShiftId, BranchId, UserId);
        //    return db.SafeBalanceReport.FromSqlRaw(SQLStatment);
        //}
    }
}
