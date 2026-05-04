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

        public IQueryable<DealerList> GetDealers(long DealerTypeId, string txtSearch)
        {
            string SQLStatment = string.Format( File.ReadAllText(Path.GetFullPath(@"SQLFiles/DealerListSql.sql")).Replace("Org" , _Schema) , txtSearch , DealerTypeId);
            return db.DealerListReport.FromSqlRaw<DealerList>(SQLStatment);
        }

        public IQueryable<ProductList> GetProducts(long ClassificationId)
        {
            var xx = File.ReadAllText(Path.GetFullPath(@"SQLFiles/ProductListSql.sql")).Replace("Org", _Schema);
            string SQLStatment = string.Format(xx, ClassificationId);
            return db.ProductListReport.FromSqlRaw<ProductList>(SQLStatment);
        }

        public IQueryable<StockList> GetStocks( string txtSearch)
        {
            string SQLStatment = string.Format(File.ReadAllText(Path.GetFullPath(@"SQLFiles/StockListSql.sql")).Replace("Org", _Schema), txtSearch);
            return db.StockListReport.FromSqlRaw<StockList>(SQLStatment);
        }  
        
        public IQueryable<SafeList> GetSafes( string txtSearch)
        {
            string SQLStatment = string.Format(File.ReadAllText(Path.GetFullPath(@"SQLFiles/SafeListSql.sql")).Replace("Org", _Schema), txtSearch);
            return db.SafeListReport.FromSqlRaw<SafeList>(SQLStatment);
        }
    }
}