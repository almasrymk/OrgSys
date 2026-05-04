using System;
using System.IO;
using System.Linq;
using Entity.ModelReport;
using Microsoft.EntityFrameworkCore;

namespace Repository.DAL.Output
{
    public class DealersStatmentRepo
    {
        public OrgContext db;
        string _Schema;
        public DealersStatmentRepo(string Schema)
        {
            _Schema = Schema;
            if (this.db == null)
                this.db = new OrgContext(new DbContextOptions<OrgContext>() , _Schema);
        }

        public IQueryable<DealerStatment> GetDealersStatment(
            long DealerTypeId, DateTime FromDate, DateTime ToDate,
            long DealerId, long ShiftId, long BranchId, long UserId
            )
        {
            string SQLStatment = string.Format(File.ReadAllText(Path.GetFullPath(@"SQLFiles/DealersStatmentSql.sql")).Replace("Org", _Schema), DealerTypeId, string.Format("{0:yyyy/MM/dd}", FromDate), string.Format("{0:yyyy/MM/dd}", ToDate), DealerId, ShiftId, BranchId, UserId);
            return db.DealerStatmentReport.FromSqlRaw(SQLStatment);
        }
    }
}