using Entity;
using System.Linq;
using X.PagedList;
using Entity.Model;
using Entity.ModelView;
using System.Collections.Generic;

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
            var Nwob = repo .AddOrUpdate(ob.Map<Invoice>());

            if (ob.Id > 0)
            {
                var ids = ob.InvoiceProductList.Select(e => e.Id).ToList();
                if (ids == null) ids = new List<long>();

                // Delete row from database
                var deleted = repoAll.invoiceProductRepo.GetList(e => e.InvoiceId == ob.Id && !ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
                if (deleted != null && deleted.Count > 0)
                    repoAll.invoiceProductRepo.ShiftDelete(deleted.Select(e => e.Id).ToList());

                foreach (var productUnit in ob.InvoiceProductList)
                {
                    var model = productUnit.Map<InvoiceProduct>();
                    model.InvoiceId = Nwob.Id;
                    model.StockId = Nwob.StockId;
                    repoAll.invoiceProductRepo.AddOrUpdate(model);
                }
                Nwob.InvoiceProducts = repoAll.invoiceProductRepo.GetList(e => e.InvoiceId == Nwob.Id, e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
            }

            if (long.Parse("0" + new PreferenceService(_Schema).GetByKey("AutoCreateTransaction", "Invoice", ob.TypeId, 0)?.Value) == 1 || ob.TransactionId > 0)
                new IntegrationServics(_Schema).CreateTransactionByInvoice(Nwob.Map<InvoiceModelView>());

            if (ob.Cash)
                new IntegrationServics(_Schema).CreateFinancialByInvoice(Nwob.Map<InvoiceModelView>());

            return Nwob.Map<InvoiceModelView>();
        }

        public override bool Delete(long id)
        {
            var ob = repo .Get(e => e.Id == id);
            if (ob != null)
            {
                repo .Delete(id);
                new IntegrationServics(_Schema).DeleteInvoice(id);
                new TransactionService(_Schema).Delete(ob.TransactionId ?? 0);
                var fi = new FinancialService(_Schema).GetByParent(id);
                if (fi != null)
                    new FinancialService(_Schema).Delete(fi.Id);
                return true;
            }
            return false;
        }

        public override bool Delete(List<long> ids)
        {
            var oblist = repo .GetList(e => ids.Contains(e.Id), null, "", Utility.Status.New);
            if (oblist != null && oblist.Count() > 0)
            {
                repo .Delete(ids);
                new TransactionService(_Schema).Delete(oblist.Select(e => e.TransactionId ?? 0).ToList());
                foreach (var id in ids)
                {
                    new IntegrationServics(_Schema).DeleteInvoice(id);
                    var fi = new FinancialService(_Schema).GetByParent(id);
                    if (fi != null)
                        new FinancialService(_Schema).Delete(fi.Id);
                }

                return true;
            }
            return false;
        }

        public void Cancel(long Id)
        {
            var ob = repo.Get(e => e.Id == Id);
            ob.Status = Utility.Status.Cancel;
            ob = repo.AddOrUpdate(ob);

            if (ob.TransactionId > 0)
            {
                var ob1 = repoAll.transactionRepo.Get(e => e.Id == ob.TransactionId);
                ob1.Status = Utility.Status.Cancel;
                repoAll.transactionRepo.AddOrUpdate(ob1);
            }
        }

        public void Redo(long Id)
        {
            var ob = repo.Get(e => e.Id == Id);
            ob.Status = Utility.Status.All;
            ob = repo.AddOrUpdate(ob);

            if (ob.TransactionId > 0)
            {
                var ob1 = repoAll.transactionRepo.Get(e => e.Id == ob.TransactionId);
                ob1.Status = Utility.Status.All;
                repoAll.transactionRepo.AddOrUpdate(ob1);
            }
        }

        public List<InvoiceModelView> GetInvoicesNotReturn(string txtSearch = "", long TypeId = 0, long InvId = 0, int page = 1, int pageSize = 20)
        {
            var obList = repoAll.invoiceRepo.GetInvoicesNotReturn(txtSearch, TypeId, InvId, page, pageSize);
            if (obList == null)
                obList = new List<Invoice>();
            return obList.Select(e => e.Map<InvoiceModelView>()).ToList();
        }

        public IPagedList<InvoiceModelView> GetCreditAllByDealerId(string textSearch, long dealerId, long currencyId, string ids, long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            if (ids == null)
                ids = "";
            var idsList = ids.Split(",").Where(e => e != "").ToList();
            if (idsList == null)
                idsList = new List<string>();
            return repo.GetList(e => !ids.Contains(e.Id.ToString()) && e.TypeId == TypeId && e.DealerId == dealerId && e.CurrencyId == currencyId && e.Credit > 0 && ("" + textSearch == "" || e.Code.Contains("" + textSearch) || e.Dealer.Name.Contains("" + textSearch)), e => e.OrderByDescending(e => e.Id), Includes, Utility.Status.New).Select(e => e.Map<InvoiceModelView>()).ToPagedList(page, pageSize);
        }

        public override InvoiceModelView Get(long Id)
        {
            var ob =  base.Get(Id);
            if (ob == null)
                ob = new InvoiceModelView() { InvoiceProductList = new List<InvoiceProductModelView>() };
            foreach (var products in ob.InvoiceProductList)
            {
                products.UnitList = products.Product.ProductUnits.Select(e => e.Unit.Map<UnitModelView>()).ToList();
            }
            return ob;
        }

        public List<InvoiceProductModelView> GetProductInvoicesNotReturn(long Id)
        {
            var Invlist = repoAll.invoiceRepo.GetList(e => e.ParentId == Id, "InvoiceProducts").ToList();
            List<InvoiceProduct> proList = new List<InvoiceProduct>();
            foreach (var item in Invlist)
                proList.AddRange(item.InvoiceProducts);
            
            var ob = repoAll.invoiceRepo.Get(e=>e.Id == Id  , "InvoiceProducts");
            if (ob == null)
                ob = new Invoice() { InvoiceProducts = new List<InvoiceProduct>() };
            foreach (var item in ob.InvoiceProducts)
                item.Quantity -= proList.Where(e => e.ProductId == item.ProductId)?.Sum(e => e.Quantity)??0;
            
            return ob.InvoiceProducts.Select(e=>e.Map<InvoiceProductModelView>()).ToList();
        }
    }
}