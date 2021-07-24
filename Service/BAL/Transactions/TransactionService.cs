using Entity.Model;
using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class TransactionService : BaseService<TransactionModelView>
    {
        string Includes = "Dealer,Store,ToStore,TransactionProducts,TransactionProducts.Product,TransactionProducts.Product.ProductUnits,,TransactionProducts.Product.ProductUnits.Unit";
        UnitOfWork repo;
        public TransactionService()
        {
            repo = new UnitOfWork();
        }

        #region Save / Delete
        public TransactionModelView Save(TransactionModelView ob)
        {
            if (ob.Id > 0)
            {
                ob.ParentId = Get(ob.Id).ParentId;
            }

            // Save
            var Nwob = repo.transactionRepo.AddOrUpdate(ob.Model);

            if (ob.Id > 0)
            {
                var ids = ob.TransactionProducts.Select(e => e.Id).ToList();
                if (ids == null) ids = new List<long>();

                // Delete row from database
                var deleted = repo.transactionProductRepo.GetList(e => e.TransactionId == ob.Id && !ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
                if (deleted != null && deleted.Count > 0)
                    repo.transactionProductRepo.ShiftDelete(deleted.Select(e => e.Id).ToList());

                foreach (var productUnit in ob.TransactionProducts)
                {
                    var model = productUnit.Model;
                    model.TransactionId = Nwob.Id;
                    repo.transactionProductRepo.AddOrUpdate(model);
                }
                Nwob.TransactionProducts = repo.transactionProductRepo.GetList(e => e.TransactionId == Nwob.Id, e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
            }

            if (ob.TypeId == 3)
            {
                var setting = new PreferenceService();
                if (long.Parse("0" + setting.GetByKey("AutoReceived", "Transaction", ob.TypeId, 0)?.Value) == 1 || ob.ParentId > 0)
                    CreateReceived(Nwob);
            }

            return new TransactionModelView(Nwob);
        }               
         
        public bool Delete(long id)
        {
            return repo.transactionRepo.Delete(id);
        }

        public bool Delete(List<long> ids)
        {
            return repo.transactionRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public TransactionModelView Get(long Id)
        {
            var ob = new TransactionModelView(repo.transactionRepo.Get(e => e.Id == Id, Includes));
            return GetParent(ob);
        }

        public TransactionModelView Get(string textSearch)
        {
            var ob = new TransactionModelView(repo.transactionRepo.Get(e => e.Dealer.Name.Contains("" + textSearch), Includes));
            return GetParent(ob);
        }

        public List<TransactionModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            var obList = repo.transactionRepo.GetList( e=>e.TypeId == TypeId, e => e.OrderByDescending(e => e.Id), Includes, Utility.Status.New).Select(e => new TransactionModelView(e)).ToList();
            return GetParent(obList);
        }
         
        public List<TransactionModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            var obList= repo.transactionRepo.GetList(e => e.Dealer.Name.Contains("" + textSearch) && e.TypeId == TypeId, e => e.OrderByDescending(e => e.Id), Includes, Utility.Status.New).Select(e => new TransactionModelView(e)).ToList();
            return GetParent(obList);
        }
         
        public IPagedList<TransactionModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            var obList = repo.transactionRepo.GetList(e=> e.TypeId == TypeId , e => e.OrderByDescending(e => e.Id), Includes, Utility.Status.New).Select(e => new TransactionModelView(e)).ToPagedList(page, pageSize);
            return GetParent(obList);
        }
         
        public IPagedList<TransactionModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            var obList = repo.transactionRepo.GetList(e => e.TypeId == TypeId && ("" + textSearch == "" || e.Dealer.Name.Contains("" + textSearch)), e => e.OrderByDescending(e => e.Id), Includes, Utility.Status.New).Select(e => new TransactionModelView(e)).ToPagedList(page, pageSize);
            return GetParent(obList);
        }
        
        public List<TransactionModelView> GetAll(List<long> ids,long TypeId = 0)
        {
            var obList = repo.transactionRepo.GetList(e => e.TypeId == TypeId && ids.Contains(e.Id), e => e.OrderBy(e => e.Id) , Includes, Utility.Status.New).Select(e => new TransactionModelView(e)).ToList();
            return GetParent(obList);
        }      

        public TransactionModelView GetParent(TransactionModelView ob)
        {
            ob.ParentCode = repo.transactionRepo.Get(e => (e.Id == ob.ParentId && ob.TypeId == 4) || (e.ParentId == ob.Id && ob.TypeId == 3))?.Code;
            return ob;
        }

        public List<TransactionModelView> GetParent(List<TransactionModelView> obList)
        {
            foreach (var ob in obList)
            {
                ob.ParentCode = repo.transactionRepo.Get(e => (e.Id == ob.ParentId && ob.TypeId == 4) || (e.ParentId == ob.Id && ob.TypeId == 3))?.Code;
            }
            return obList;
        }

        public IPagedList<TransactionModelView> GetParent(IPagedList<TransactionModelView> obList)
        {
            foreach (var ob in obList)
            {
                ob.ParentCode = repo.transactionRepo.Get(e => (e.Id == ob.ParentId && ob.TypeId == 4) || (e.ParentId == ob.Id && ob.TypeId == 3))?.Code;
                    }
            return obList;
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.transactionRepo.GetMaXCode(e => e.TypeId == type);
        }
        #endregion

        #region Integration
        public bool CreateTransactionByInvoice(InvoiceModelView inv)
        {
            if (inv != null && inv.Id > 0 && inv.Credit > 0)
            {
                var transaction = new TransactionModelView(inv.Model);               
                inv.CodeNumber = GetMaxCode(inv.TypeId == 2 || inv.TypeId == 3 ? 1 : 2);
                inv.Code = "" + inv.CodeNumber;
                Save(transaction);
                return true;
            }
            return false;
        }

        public bool CreateReceived(Transaction transaction)
        {
            if (transaction != null && transaction.Id > 0)
            {
                var received = new TransactionModelView(transaction);
                received.TypeId = 4;
                foreach (var item in received.TransactionProducts)
                    item.TypeId = 4;
                received.CodeNumber = GetMaxCode(4);
                received.Code = "" + received.CodeNumber;
                Save(received);
                return true;
            }
            return false;
        }
        #endregion
    }
}