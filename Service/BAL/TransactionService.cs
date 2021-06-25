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
    public class TransactionService : BaseService<TransactionModelView>
    {
        UnitOfWork repo;
        public TransactionService()
        {
            repo = new UnitOfWork();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
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
                {
                    Nwob = CreateTransaction(Nwob);
                }
            }

            return new TransactionModelView(Nwob);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Transaction CreateTransaction(long Id)
        {
            var ob = Get(Id);
            return CreateTransaction(ob.Model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public Transaction CreateTransaction(Transaction ob)
        {
            if (ob != null)
            {
                var trns = repo.transactionRepo.Get(e => e.ParentId == ob.Id);
                if (trns == null || trns.Id == 0)
                {
                    long trnsTypeId = 4;

                    trns = new Transaction();
                    var setting = new PreferenceService();
                    trns.StoreId = ob.ToStoreId.Value;
                    trns.TypeId = trnsTypeId;
                    trns.ParentId = ob.Id;
                    trns.CodeNumber = new TransactionService().GetMaxCode(trnsTypeId);
                    trns.Code = "" + new TransactionService().GetMaxCode(trnsTypeId);
                }
                var obInv = new TransactionService().Save(new TransactionModelView(trns).UpdateData(ob));               
            }
            return ob;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(long id)
        {
            return repo.transactionRepo.Delete(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TransactionModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            var obList = repo.transactionRepo.GetList( e=>e.TypeId == TypeId, e => e.OrderByDescending(e => e.Id), "Dealer,Store,ToStore", Utility.Status.New).Select(e => new TransactionModelView(e)).ToList();
            return GetParent(obList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<TransactionModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            var obList= repo.transactionRepo.GetList(e => e.Dealer.Name.Contains("" + textSearch) && e.TypeId == TypeId, e => e.OrderByDescending(e => e.Id), "Dealer,Store,ToStore", Utility.Status.New).Select(e => new TransactionModelView(e)).ToList();
            return GetParent(obList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<TransactionModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            var obList = repo.transactionRepo.GetList(e=> e.TypeId == TypeId , e => e.OrderByDescending(e => e.Id), "Dealer,Store,ToStore", Utility.Status.New).Select(e => new TransactionModelView(e)).ToPagedList(page, pageSize);
            return GetParent(obList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<TransactionModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            var obList = repo.transactionRepo.GetList(e => e.TypeId == TypeId && ("" + textSearch == "" || e.Dealer.Name.Contains("" + textSearch)), e => e.OrderByDescending(e => e.Id), "Dealer,Store,ToStore", Utility.Status.New).Select(e => new TransactionModelView(e)).ToPagedList(page, pageSize);
            return GetParent(obList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TransactionModelView Get(long Id)
        {
            var ob = new TransactionModelView(repo.transactionRepo.Get(e => e.Id == Id  , "Dealer,Store,ToStore,TransactionProducts,TransactionProducts.Product,TransactionProducts.Product.ProductUnits,,TransactionProducts.Product.ProductUnits.Unit"));
            return GetParent(ob);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public TransactionModelView Get(string textSearch)
        {
            var ob = new TransactionModelView(repo.transactionRepo.Get(e => e.Dealer.Name.Contains("" + textSearch) , "Dealer,Store,ToStore"));
            return GetParent(ob);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(List<long> ids)
        {
            return repo.transactionRepo.Delete(ids);
        }

        public List<TransactionModelView> GetAll(List<long> ids,long TypeId = 0)
        {
            var obList = repo.transactionRepo.GetList(e => e.TypeId == TypeId && ids.Contains(e.Id), e => e.OrderBy(e => e.Id) , "Dealer,Store,ToStore", Utility.Status.New).Select(e => new TransactionModelView(e)).ToList();
            return GetParent(obList);
        }
       // , long TypeId
        public long GetMaxCode(long type )
        {
            return repo.transactionRepo.GetMaXCode(type);
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
    }
}