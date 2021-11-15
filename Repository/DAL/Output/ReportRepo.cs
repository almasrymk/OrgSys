using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Utility;
using Entity.ModelReport;
using Microsoft.Extensions.Configuration;

namespace Repository
{
    public class ReportRepo
    {
        public OrgContext db;

        public ReportRepo(string Schema)
        {
            if (this.db == null)
                this.db = new OrgContext(new MyDbContextOptions<OrgContext> { Schema = Schema });
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
            return db.Dealers.Where(r => (r.Id == dealerId || dealerId==0) && r.TypeId == typeId).Select(e => new Customer(e));
        }
    }
}