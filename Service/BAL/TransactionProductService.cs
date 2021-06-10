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
    public class TransactionProductService : BaseService<TransactionProductModelView>
    {
        UnitOfWork repo;
        public TransactionProductService()
        {
            repo = new UnitOfWork();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public TransactionProductModelView Save(TransactionProductModelView ob)
        {
            return new TransactionProductModelView(repo.transactionProductRepo.AddOrUpdate(ob.Model));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(long id)
        {
            return repo.transactionProductRepo.Delete(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TransactionProductModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.transactionProductRepo.GetList(e => e.OrderBy(e => e.Id), "Product", Utility.Status.New).Select(e => new TransactionProductModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<TransactionProductModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.transactionProductRepo.GetList(e => e.Product.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "Product", Utility.Status.New).Select(e => new TransactionProductModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<TransactionProductModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.transactionProductRepo.GetList(e => e.OrderBy(e => e.Id), "Product", Utility.Status.New).Select(e => new TransactionProductModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<TransactionProductModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.transactionProductRepo.GetList(e => "" + textSearch == "" || e.Product.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "Product", Utility.Status.New).Select(e => new TransactionProductModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TransactionProductModelView Get(long Id)
        {
            return new TransactionProductModelView(repo.transactionProductRepo.Get(e => e.Id == Id  , "Product"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public TransactionProductModelView Get(string textSearch)
        {
            return new TransactionProductModelView(repo.transactionProductRepo.Get(e => e.Product.Name.Contains("" + textSearch) , "Product"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(List<long> ids)
        {
            return repo.transactionProductRepo.Delete(ids);
        }

        public List<TransactionProductModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.transactionProductRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id) , "Product", Utility.Status.New).Select(e => new TransactionProductModelView(e)).ToList();
        }
    }
}