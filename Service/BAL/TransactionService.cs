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
            return new TransactionModelView(Nwob);
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
            return repo.transactionRepo.GetList( e=>e.TypeId == TypeId, e => e.OrderByDescending(e => e.Id), "Dealer", Utility.Status.New).Select(e => new TransactionModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<TransactionModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.transactionRepo.GetList(e => e.Dealer.Name.Contains("" + textSearch) && e.TypeId == TypeId, e => e.OrderByDescending(e => e.Id), "Dealer", Utility.Status.New).Select(e => new TransactionModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<TransactionModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.transactionRepo.GetList(e=> e.TypeId == TypeId , e => e.OrderByDescending(e => e.Id), "Dealer", Utility.Status.New).Select(e => new TransactionModelView(e)).ToPagedList(page, pageSize);
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
            return repo.transactionRepo.GetList(e => e.TypeId == TypeId && ("" + textSearch == "" || e.Dealer.Name.Contains("" + textSearch)), e => e.OrderByDescending(e => e.Id), "Dealer", Utility.Status.New).Select(e => new TransactionModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TransactionModelView Get(long Id)
        {
            return new TransactionModelView(repo.transactionRepo.Get(e => e.Id == Id  , "Dealer,TransactionProducts,TransactionProducts.Product,TransactionProducts.Product.ProductUnits,,TransactionProducts.Product.ProductUnits.Unit"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public TransactionModelView Get(string textSearch)
        {
            return new TransactionModelView(repo.transactionRepo.Get(e => e.Dealer.Name.Contains("" + textSearch) , "Dealer"));
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
            return repo.transactionRepo.GetList(e => e.TypeId == TypeId && ids.Contains(e.Id), e => e.OrderBy(e => e.Id) , "Dealer", Utility.Status.New).Select(e => new TransactionModelView(e)).ToList();
        }
       // , long TypeId
        public long GetMaxCode(long type )
        {
            return repo.transactionRepo.GetMaXCode(type);
        }
    }
}