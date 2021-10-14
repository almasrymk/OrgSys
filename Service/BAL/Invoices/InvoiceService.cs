using Entity.Model;
using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class InvoiceService : BaseService<InvoiceModelView>
    {
        string Includes = "Dealer,Transaction,InvoiceProducts,InvoiceProducts.Product,InvoiceProducts.Product.ProductUnits,,InvoiceProducts.Product.ProductUnits.Unit";
        UnitOfWork repo;
        public InvoiceService()
        {
            repo = new UnitOfWork();
        }

        #region Save / Delete
        public InvoiceModelView Save(InvoiceModelView ob)
        {
            if (ob.Id > 0)
            {
                ob.TransactionId = Get(ob.Id).TransactionId;
            }

            // Save
            var Nwob = repo.invoiceRepo.AddOrUpdate(ob.Model);

            if (ob.Id > 0)
            {
                var ids = ob.InvoiceProducts.Select(e => e.Id).ToList();
                if (ids == null) ids = new List<long>();

                // Delete row from database
                var deleted = repo.invoiceProductRepo.GetList(e => e.InvoiceId == ob.Id && !ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
                if (deleted != null && deleted.Count > 0)
                    repo.invoiceProductRepo.ShiftDelete(deleted.Select(e => e.Id).ToList());

                foreach (var productUnit in ob.InvoiceProducts)
                {
                    var model = productUnit.Model;
                    model.InvoiceId = Nwob.Id;
                    model.StoreId = Nwob.StoreId;
                    repo.invoiceProductRepo.AddOrUpdate(model);
                }
                Nwob.InvoiceProducts = repo.invoiceProductRepo.GetList(e => e.InvoiceId == Nwob.Id, e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
            }

            if (long.Parse("0" + new PreferenceService().GetByKey("AutoCreateTransaction", "Invoice", ob.TypeId, 0)?.Value) == 1 || ob.TransactionId > 0)
                new IntegrationServics().CreateTransactionByInvoice(new InvoiceModelView(Nwob));

            if (ob.Cash)
                new IntegrationServics().CreateFinancialByInvoice(new InvoiceModelView(Nwob));

            return new InvoiceModelView(Nwob);
        }    

        public bool Delete(long id)
        {
            var ob = repo.invoiceRepo.Get(e => e.Id == id);
            if (ob != null)
            {
                repo.invoiceRepo.Delete(id);
                new IntegrationServics().DeleteInvoice(id);
                new TransactionService().Delete(ob.TransactionId ?? 0);
                var fi = new FinancialService().GetByParent(id);
                new FinancialService().Delete(fi.Id);
                return true;
            }
            return false;
        }

        public bool Delete(List<long> ids)
        {
            var oblist = repo.invoiceRepo.GetList(e => ids.Contains(e.Id), null, "", Utility.Status.New);
            if (oblist != null && oblist.Count() > 0)
            {
                repo.invoiceRepo.Delete(ids);               
                new TransactionService().Delete(oblist.Select(e => e.TransactionId ?? 0).ToList());
                foreach (var id in ids)
                {
                    new IntegrationServics().DeleteInvoice(id);
                    var fi = new FinancialService().GetByParent(id);
                    new FinancialService().Delete(fi.Id);
                }

                return true;
            }
            return false;
        }

        public void Cancel(long Id)
        {
            var ob = repo.invoiceRepo.Get(e => e.Id == Id);
            ob.Status = Utility.Status.Cancel;           
            ob = repo.invoiceRepo.AddOrUpdate(ob);
        }

        public void Redo(long Id)
        {
            var ob = repo.invoiceRepo.Get(e => e.Id == Id);
            ob.Status = Utility.Status.All;           
            ob = repo.invoiceRepo.AddOrUpdate(ob);
        }
        #endregion

        #region Gets
        public InvoiceModelView Get(long Id)
        {
            return new InvoiceModelView(repo.invoiceRepo.Get(e => e.Id == Id, Includes));
        }

        public InvoiceModelView Get(string textSearch)
        {
            return new InvoiceModelView(repo.invoiceRepo.Get(e => e.Dealer.Name.Contains("" + textSearch), Includes));
        }

        public List<InvoiceModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.invoiceRepo.GetList(e => e.TypeId == TypeId, e => e.OrderByDescending(e => e.Id), Includes, Utility.Status.New).Select(e => new InvoiceModelView(e)).ToList();
        }

        public List<InvoiceModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0)
        {
            return repo.invoiceRepo.GetList(e => e.Dealer.Name.Contains("" + textSearch) && e.TypeId == TypeId, e => e.OrderByDescending(e => e.Id), Includes, Utility.Status.New).Select(e => new InvoiceModelView(e)).ToList();
        }

        public IPagedList<InvoiceModelView> GetAll(long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.invoiceRepo.GetList(e => e.TypeId == TypeId, e => e.OrderByDescending(e => e.Id), Includes, Utility.Status.New).Select(e => new InvoiceModelView(e)).ToPagedList(page, pageSize);
        }

        public IPagedList<InvoiceModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.invoiceRepo.GetList(e => e.TypeId == TypeId && ("" + textSearch == "" || e.Dealer.Name.Contains("" + textSearch)), e => e.OrderByDescending(e => e.Id), Includes, Utility.Status.New).Select(e => new InvoiceModelView(e)).ToPagedList(page, pageSize);
        }
        public IPagedList<InvoiceModelView> GetCreditAllByDealerId(string textSearch, long dealerId, long currencyId, string ids, long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            if (ids == null)
                ids = "";
            var idsList = ids.Split(",").Where(e => e != "").ToList();
            if (idsList == null)
                idsList = new List<string>();
            return repo.invoiceRepo.GetList(e => !ids.Contains(e.Id.ToString()) && e.TypeId == TypeId && e.DealerId == dealerId && e.CurrencyId == currencyId && e.Credit > 0 && ("" + textSearch == "" || e.Code.Contains("" + textSearch) || e.Dealer.Name.Contains("" + textSearch)), e => e.OrderByDescending(e => e.Id), Includes, Utility.Status.New).Select(e => new InvoiceModelView(e)).ToPagedList(page, pageSize);
        }

        public List<InvoiceModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.invoiceRepo.GetList(e => e.TypeId == TypeId && ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new InvoiceModelView(e)).ToList();
        }                

        public List<InvoiceModelView> GetInvoicesNotReturn(string txtSearch = "", long TypeId = 0, long InvId = 0, int page = 1, int pageSize = 20)
        {
            var obList = repo.invoiceRepo.GetInvoicesNotReturn(txtSearch, TypeId, InvId, page, pageSize);
            if (obList == null)
                obList = new List<Invoice>();
            return obList.Select(e => new InvoiceModelView(e)).ToList();
        }

        public long GetMaxCode(long type)
        {
            return repo.invoiceRepo.GetMaXCode(e => e.TypeId == type);
        }
        #endregion       
    }
}