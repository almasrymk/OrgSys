using Entity.ModelReport;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Repository.DAL.Output
{
    public class LookupsRepo
    {
        public OrgContext db;
        string _Schema;
        public LookupsRepo(string Schema)
        {
            _Schema = Schema;
            if (this.db == null)
                this.db = new OrgContext(new DbContextOptions<OrgContext>() , _Schema);
        }

        public IQueryable<DealerListReport> GetDealers(long DealerTypeId, string txtSearch)
        {
            string SQLStatment = string.Format( File.ReadAllText(Path.GetFullPath(@"SQLFiles/DealerListSql.sql")).Replace("Org" , _Schema) , txtSearch , DealerTypeId);
            return db.DealerListReport.FromSqlRaw<DealerListReport>(SQLStatment);
        }
    }
}