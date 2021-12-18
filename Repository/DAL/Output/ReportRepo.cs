using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Utility;
using Entity.ModelReport;
using Microsoft.Extensions.Configuration;
using Entity.ModelView;
using Entity.Model;

namespace Repository
{
    public class ReportRepo
    {
        public OrgContext db;

        public ReportRepo(string Schema)
        {
            if (this.db == null)
                this.db = new OrgContext(new DbContextOptions<OrgContext>(), Schema);
        }

        public IQueryable<InvoiceDetail> InvoiceDetails(long typeId, DateTime fromDate, DateTime toDate, long dealerId, long shiftId, long branchId, long userId)
        {
            long typeReturnId = typeId == 1 ? 3 : 4;
            List<long> ids = db.LogSys.Where(e => e.UserId == userId && e.TableName == "Invoice" && e.LogType == LogType.Add).Select(e => long.Parse("0" + e.ResourceId)).ToList();
            return db.Invoices
                .Include("Store")
                .Include("Store.Branch")
                .Include("Dealer")
                .Include("Shift")
                .Where(e =>
            (e.TypeId == typeId || e.TypeId == typeReturnId) &&
            (e.Date >= fromDate && e.Date <= toDate) &&
            (dealerId == 0 || e.DealerId == dealerId) &&
            (shiftId == 0 || e.ShiftId == shiftId) &&
            (ids.Contains(e.Id) || userId == 0) &&
            (branchId == 0 || e.Store.BranchId == branchId) &&
            (e.Status != Status.Deleted && e.Hide != true)
            ).OrderByDescending(e => e.Date).Select(e => new InvoiceDetail(e));





        }
        public IQueryable<Customer> Customer(long typeId, long dealerId)
        {
            return db.Dealers.Where(r => (r.Id == dealerId || dealerId == 0) && r.TypeId == typeId).Select(e => new Customer(e));
        }


        public List<DealerInvoice> TotalInvoicCustomer(long typeId, DateTime fromDate, DateTime toDate, long dealerId)
        {
            return db.Invoices.Where(e =>
            e.TypeId == typeId &&
            e.Date >= fromDate && e.Date <= toDate &&
            (dealerId == 0 || e.DealerId == dealerId))
            .GroupBy(a => new { a.Dealer.Name, a.Dealer.Code, a.Dealer.Id })
            .Select(a => new DealerInvoice { Total = a.Sum(b => b.Net), Name = a.Key.Name, Code = a.Key.Code, Id = a.Key.Id })
            .OrderByDescending(a => a.Code)
            .ToList();
        }


        public List<SupplierSheetReport> SuplierSheetReport(long typeId, DateTime fromDate, DateTime toDate, long dealerId, int typeinvoiceorpinvoice, int typeReturninvoiceorpinvoice)
        {

            var kgt = db.Invoices.Where(e =>
              e.TypeId == typeinvoiceorpinvoice &&
              e.Date >= fromDate && e.Date <= toDate &&
              (dealerId == 0 || e.DealerId == dealerId))
               .GroupBy(a => new { a.Dealer.Name, a.Dealer.Code, a.DealerId, a.Date, a.CodeNumber })
               .Select(a => new SupplierSheetReport
               {
                   Debit  = a.Sum(b => b.Net)
                   ,
                   Credit = 0
                   ,DealarName = a.Key.Name
                   ,DealarCode = a.Key.Code
                   , DealarId = (int)a.Key.DealerId
                   ,GetDateTime = a.Key.Date.Date
                   ,InvoiceCode = (int?)a.Key.CodeNumber
                   ,TypeInvoice = Utility.Resource.Title_Designer.Invoice
               })
               .Union(db.Invoices.Where(e =>
               e.TypeId == typeReturninvoiceorpinvoice &&
               e.Date >= fromDate && e.Date <= toDate &&
               (dealerId == 0 || e.DealerId == dealerId))
               .GroupBy(a => new { a.Dealer.Name, a.Dealer.Code, a.DealerId, a.Date, a.CodeNumber })
               .Select(a => new SupplierSheetReport { Debit=0
               , Credit = a.Sum(c => c.Net)
               , DealarName = a.Key.Name
               , DealarCode = a.Key.Code
               ,  DealarId = (int)a.Key.DealerId
               , GetDateTime = a.Key.Date.Date
               , InvoiceCode = (int?)a.Key.CodeNumber
               , TypeInvoice = Utility.Resource.Title_Designer.ReturnInvoices 
               }))
               .Union(db.Dealers.Where(e=>(dealerId == 0 || e.Id == dealerId) && e.TypeId == typeId).GroupBy(a=>new { a.Name,a.Code,a.Id}).Select(a => new SupplierSheetReport
               {
                   Debit = db.Invoices.Where(e => (e.DealerId == dealerId || dealerId == 0) && e.Date < fromDate && e.TypeId == typeinvoiceorpinvoice).Select(a => a.Net).Sum()
               ,
                   Credit = db.Invoices.Where(e => (e.DealerId == dealerId || dealerId == 0) && e.Date < fromDate && e.TypeId == typeReturninvoiceorpinvoice).Select(a => a.Net).Sum(),
                   DealarName = a.Key.Name
               ,
                   DealarCode = a.Key.Code
               ,
                    DealarId = (int)a.Key.Id
               ,
                   GetDateTime = fromDate.AddDays(-1)
               ,
                   InvoiceCode = -1
               ,
                   TypeInvoice = Utility.Resource.Title_Designer.BeginBalance
               }))


               .OrderBy(a => a.InvoiceCode).ThenBy(c=>c.DealarCode)
               .ToList();
            return kgt;
        }
    }
}