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
    public class TransactionTypeService : BaseService<TransactionTypeModelView>
    {
        UnitOfWork repo;
        public TransactionTypeService()
        {
            if (repo == null)
                repo = new UnitOfWork();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public TransactionTypeModelView Save(TransactionTypeModelView ob)
        {
            return new TransactionTypeModelView(repo.transactionTypeRepo.AddOrUpdate(ob.Model));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(long id)
        {
            return repo.transactionTypeRepo.Delete(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TransactionTypeModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.transactionTypeRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new TransactionTypeModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<TransactionTypeModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.transactionTypeRepo.GetList(e => e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new TransactionTypeModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<TransactionTypeModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.transactionTypeRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new TransactionTypeModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<TransactionTypeModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.transactionTypeRepo.GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new TransactionTypeModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TransactionTypeModelView Get(long Id)
        {
            return new TransactionTypeModelView(repo.transactionTypeRepo.Get(e => e.Id == Id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public TransactionTypeModelView Get(string textSearch)
        {
            return new TransactionTypeModelView(repo.transactionTypeRepo.Get(e => e.Name.Contains("" + textSearch)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(List<long> ids)
        {
            return repo.transactionTypeRepo.Delete(ids);
        }

        public List<TransactionTypeModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.transactionTypeRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new TransactionTypeModelView(e)).ToList();
        }
    }
}