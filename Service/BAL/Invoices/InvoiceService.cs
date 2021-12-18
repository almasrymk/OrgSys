using Entity;
using Entity.Model;
using Entity.ModelView;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class InvoiceService : BaseOrgService<InvoiceModelView, Invoice>
    {
        public InvoiceService(string Schema) : base(Schema, "Dealer,Transaction,InvoiceProducts,InvoiceProducts.Product,InvoiceProducts.Product.ProductUnits,,InvoiceProducts.Product.ProductUnits.Unit") { }

        public override InvoiceModelView Save(InvoiceModelView ob)
        {
            if (ob.Id > 0)
            {
                ob.TransactionId = Get(ob.Id).TransactionId;
            }

            // Save
            var Nwob = repo.GetRepo<Invoice>().AddOrUpdate(ob.Map<Invoice>());

            if (ob.Id > 0)
            {
                var ids = ob.InvoiceProducts.Select(e => e.Id).ToList();
                if (ids == null) ids = new List<long>();

                // Delete row from database
                var deleted = repo.GetRepo<InvoiceProduct>().GetList(e => e.InvoiceId == ob.Id && !ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
                if (deleted != null && deleted.Count > 0)
                    repo.GetRepo<InvoiceProduct>().ShiftDelete(deleted.Select(e => e.Id).ToList());

                foreach (var productUnit in ob.InvoiceProductList)
                {
                    var model = productUnit.Map<InvoiceProduct>();
                    model.InvoiceId = Nwob.Id;
                    model.StoreId = Nwob.StoreId;
                    repo.GetRepo<InvoiceProduct>().AddOrUpdate(model);
                }
                Nwob.InvoiceProducts = repo.GetRepo<InvoiceProduct>().GetList(e => e.InvoiceId == Nwob.Id, e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
            }

            if (long.Parse("0" + new PreferenceService(_Schema).GetByKey("AutoCreateTransaction", "Invoice", ob.TypeId, 0)?.Value) == 1 || ob.TransactionId > 0)
                new IntegrationServics(_Schema).CreateTransactionByInvoice(Nwob.Map<InvoiceModelView>());

            if (ob.Cash)
                new IntegrationServics(_Schema).CreateFinancialByInvoice(Nwob.Map<InvoiceModelView>());

            return Nwob.Map<InvoiceModelView>();
        }

        public override bool Delete(long id)
        {
            var ob = repo.GetRepo<Invoice>().Get(e => e.Id == id);
            if (ob != null)
            {
                repo.GetRepo<Invoice>().Delete(id);
                new IntegrationServics(_Schema).DeleteInvoice(id);
                new TransactionService(_Schema).Delete(ob.TransactionId ?? 0);
                var fi = new FinancialService(_Schema).GetByParent(id);
                new FinancialService(_Schema).Delete(fi.Id);
                return true;
            }
            return false;
        }

        public bool Delete(List<long> ids)
        {
            var oblist = repo.GetRepo<Invoice>().GetList(e => ids.Contains(e.Id), null, "", Utility.Status.New);
            if (oblist != null && oblist.Count() > 0)
            {
                repo.GetRepo<Invoice>().Delete(ids);
                new TransactionService(_Schema).Delete(oblist.Select(e => e.TransactionId ?? 0).ToList());
                foreach (var id in ids)
                {
                    new IntegrationServics(_Schema).DeleteInvoice(id);
                    var fi = new FinancialService(_Schema).GetByParent(id);
                    new FinancialService(_Schema).Delete(fi.Id);
                }

                return true;
            }
            return false;
        }

        public void Cancel(long Id)
        {
            var ob = repo.GetRepo<Invoice>().Get(e => e.Id == Id);
            ob.Status = Utility.Status.Cancel;
            ob = repo.GetRepo<Invoice>().AddOrUpdate(ob);
        }

        public void Redo(long Id)
        {
            var ob = repo.GetRepo<Invoice>().Get(e => e.Id == Id);
            ob.Status = Utility.Status.All;
            ob = repo.GetRepo<Invoice>().AddOrUpdate(ob);
        }
    }
}