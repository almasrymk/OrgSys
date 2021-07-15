using Entity.Model;
using Entity.ModelView;
using X.PagedList;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.BAL
{
    public class InvoiceService : BaseService<InvoiceModelView>
    {
        UnitOfWork repo;
        public InvoiceService()
        {
            repo = new UnitOfWork();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
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

            var setting = new PreferenceService();
            if (long.Parse("0" + setting.GetByKey("AutoCreateTransaction", "Invoice", ob.TypeId, 0)?.Value) == 1 || ob.TransactionId > 0)
            {
                Nwob = CreateTransaction(Nwob);
            }

            return new InvoiceModelView(Nwob);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Invoice CreateTransaction(long Id)
        {
            var ob = Get(Id);
            return CreateTransaction(ob.Model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public Invoice CreateTransaction(Invoice ob)
        {
            if (ob != null)
            {
                var trns = repo.transactionRepo.Get(e => e.Id == ob.TransactionId);
                if (trns == null || trns.Id == 0)
                {
                    long trnsTypeId = 1;
                    if (ob.TypeId == 1 || ob.TypeId == 3)
                        trnsTypeId = 2;

                    trns = new Transaction();
                    var setting = new PreferenceService();
                    trns.StoreId = long.Parse("0" + setting.GetByKey("DefaultStore", "Transaction", trnsTypeId, 0)?.Value);
                    if (ob.DealerId == 0)
                    {
                        if (trnsTypeId == 1)
                            trns.DealerId = long.Parse("0" + setting.GetByKey("DefaultCustomer", "Transaction", trnsTypeId, 0)?.Value);
                        else
                            trns.DealerId = long.Parse("0" + setting.GetByKey("DefaultSupplier", "Transaction", trnsTypeId, 0)?.Value);
                    }
                    else
                        trns.DealerId = ob.DealerId;

                    trns.TypeId = trnsTypeId;
                    trns.CodeNumber = new TransactionService().GetMaxCode(trnsTypeId);
                    trns.Code = "" + new TransactionService().GetMaxCode(trnsTypeId);
                }
                var obInv = new TransactionService().Save(new TransactionModelView(trns).UpdateData(ob));
                ob.TransactionId = obInv.Id;
                ob = repo.invoiceRepo.AddOrUpdate(ob);
            }
            return ob;
        }

        public void UpdateCredit(List<long> ids)
        {
            foreach (var id in ids)
            {
                var inv = repo.invoiceRepo.Get(e => e.Id == id);
                if (inv == null || inv.Id == 0)
                    continue;

                var amount = repo.financialInvoiceRepo.GetList(e => e.InvoiceId == id, null, "Invoice").Sum(e => e.Amount);
                inv.Credit = inv.Net - amount;
                repo.invoiceRepo.AddOrUpdate(inv);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(long id)
        {
            var ob = repo.invoiceRepo.Get(e => e.Id == id);
            if (ob != null)
            {
                repo.invoiceRepo.Delete(id);
                new TransactionService().Delete(ob.TransactionId??0);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<InvoiceModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.invoiceRepo.GetList(e => e.TypeId == TypeId, e => e.OrderByDescending(e => e.Id), "Dealer,Transaction", Utility.Status.New).Select(e => new InvoiceModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<InvoiceModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0)
        {
            return repo.invoiceRepo.GetList(e => e.Dealer.Name.Contains("" + textSearch) && e.TypeId == TypeId, e => e.OrderByDescending(e => e.Id), "Dealer,Transaction", Utility.Status.New).Select(e => new InvoiceModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<InvoiceModelView> GetAll(long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.invoiceRepo.GetList(e => e.TypeId == TypeId, e => e.OrderByDescending(e => e.Id), "Dealer,Transaction", Utility.Status.New).Select(e => new InvoiceModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<InvoiceModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.invoiceRepo.GetList(e => e.TypeId == TypeId && ("" + textSearch == "" || e.Dealer.Name.Contains("" + textSearch)), e => e.OrderByDescending(e => e.Id), "Dealer,Transaction", Utility.Status.New).Select(e => new InvoiceModelView(e)).ToPagedList(page, pageSize);
        }

        public IPagedList<InvoiceModelView> GetCreditAllByDealerId(string textSearch , long dealerId , long currencyId, string ids, long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            if (ids == null)
                ids = "";
           var idsList = ids.Split(",").Where(e => e != "").ToList();
            if (idsList == null)
                idsList = new List<string>();
            return repo.invoiceRepo.GetList(e => !ids.Contains(e.Id.ToString()) && e.TypeId == TypeId && e.DealerId == dealerId && e.CurrencyId == currencyId && e.Credit > 0 && ("" + textSearch == "" || e.Code.Contains("" + textSearch) || e.Dealer.Name.Contains("" + textSearch)), e => e.OrderByDescending(e => e.Id), "Dealer", Utility.Status.New).Select(e => new InvoiceModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InvoiceModelView Get(long Id)
        {
            return new InvoiceModelView(repo.invoiceRepo.Get(e => e.Id == Id, "Dealer,Transaction,InvoiceProducts,InvoiceProducts.Product,InvoiceProducts.Product.ProductUnits,,InvoiceProducts.Product.ProductUnits.Unit"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public InvoiceModelView Get(string textSearch)
        {
            return new InvoiceModelView(repo.invoiceRepo.Get(e => e.Dealer.Name.Contains("" + textSearch), "Dealer,Transaction"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(List<long> ids)
        {
            var oblist = repo.invoiceRepo.GetList(e => ids.Contains(e.Id), null, "", Utility.Status.New);
            if (oblist != null && oblist.Count() > 0)
            {
                repo.invoiceRepo.Delete(ids);
                new TransactionService().Delete(oblist.Select(e => e.TransactionId ?? 0).ToList());
                return true;
            }
            return false;
        }

        public List<InvoiceModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.invoiceRepo.GetList(e => e.TypeId == TypeId && ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "Dealer,Transaction", Utility.Status.New).Select(e => new InvoiceModelView(e)).ToList();
        }
        // , long TypeId
        public long GetMaxCode(long type)
        {
            return repo.invoiceRepo.GetMaXCode(type);
        }

        public List<InvoiceModelView> GetInvoicesNotReturn(string txtSearch = "" ,long TypeId = 0 , long InvId = 0 ,  int page = 1, int pageSize = 20)
        {
            var obList = repo.invoiceRepo.GetInvoicesNotReturn(txtSearch, TypeId, InvId, page, pageSize);
            if (obList == null)
                obList = new List<Invoice>();
            return obList.Select(e => new InvoiceModelView(e)).ToList();
        }
    }
}