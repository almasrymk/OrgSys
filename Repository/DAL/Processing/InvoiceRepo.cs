using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Utility;

namespace Repository
{
    public class InvoiceRepo : CurdOrg<Invoice>
    {
        public InvoiceRepo(string Schema) : base(Schema) { }

        public override IQueryable<Invoice> GetList(System.Func<IQueryable<Invoice>, IOrderedQueryable<Invoice>> orderBy, string includeProperties = "", Status status = Status.All)
        {
            return base.GetList(a=>a.OrderByDescending(e=>e.Id), includeProperties, status);
        }

        public override IQueryable<Invoice> GetList(Expression<System.Func<Invoice, bool>> filter, System.Func<IQueryable<Invoice>, IOrderedQueryable<Invoice>> orderBy, string includeProperties = "", Status status = Status.All)
        {
            return base.GetList(filter, a => a.OrderByDescending(e => e.Id), includeProperties, status);
        }

        public List<Invoice> GetInvoicesNotReturn(string txtSearch = "" , long TypeId = 0 , long InvId = 0 , int page = 1 , int pageSize = 20)
        {
            List<long> ids = new List<long>();
            List<long> idInvs = new List<long>();
            var invs = new List<Invoice>();
            var invsProReurn = new List<InvoiceProduct>();

            // Get all product return
            invsProReurn = db.InvoiceProducts.Include("Invoice").Where(e => e.Invoice.TypeId == TypeId && e.Invoice.ParentId > 0).ToList();
            if (invsProReurn == null)
                invsProReurn = new List<InvoiceProduct>();

            // Get invoices ids is return
            idInvs = invsProReurn.Select(e => e.Invoice.ParentId).ToList();

            // Get invoices is reurn
            invs = db.Invoices.Include("InvoiceProducts").Where(e => idInvs.Contains(e.Id)).ToList();

            // Compare quantity return
            foreach (var inv in invs)
            {
                bool AddDo = true;
                foreach (var InvoiceProduct in inv.InvoiceProducts)
                {
                    var q = invsProReurn.Where(w => w.InvoiceId == inv.Id && w.ProductId == InvoiceProduct.ProductId).Sum(a => a.Quantity);
                    if (q < InvoiceProduct.Quantity)
                    {
                        AddDo = false;
                        break;
                    }
                }               

                if (AddDo)
                    ids.Add(inv.Id);
            }
            
            // Get all invoices no return
            return db.Invoices.Where(e => ("" + txtSearch == "" || e.Code.Contains(txtSearch)) && (e.TypeId== (TypeId - 2) && !ids.Contains(e.Id)) || e.Id == InvId).Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}