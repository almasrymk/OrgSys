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
    public class CurrencyService : BaseService<CurrencyModelView>
    {
        UnitOfWork repo;
        public CurrencyService()
        {
            if (repo == null)
                repo = new UnitOfWork();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public CurrencyModelView Save(CurrencyModelView ob)
        {
            ob = new CurrencyModelView(repo.currencyRepo.AddOrUpdate(ob.Model));
            if(ob.IsDefault )
            {
                 var obOldIsDefault = repo.currencyRepo.Get(e => e.IsDefault && e.Id != ob.Id);
                if(obOldIsDefault != null && ob.Id != obOldIsDefault.Id)
                {
                    obOldIsDefault.IsDefault = false;
                    repo.currencyRepo.AddOrUpdate(obOldIsDefault);
                }
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
            return repo.currencyRepo.Delete(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<CurrencyModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.currencyRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new CurrencyModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<CurrencyModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.currencyRepo.GetList(e => e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new CurrencyModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<CurrencyModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.currencyRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new CurrencyModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<CurrencyModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.currencyRepo.GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new CurrencyModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CurrencyModelView Get(long Id)
        {
            return new CurrencyModelView(repo.currencyRepo.Get(e => e.Id == Id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public CurrencyModelView Get(string textSearch)
        {
            return new CurrencyModelView(repo.currencyRepo.Get(e => e.Name.Contains("" + textSearch)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(List<long> ids)
        {
            return repo.currencyRepo.Delete(ids);
        }

        public List<CurrencyModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.currencyRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new CurrencyModelView(e)).ToList();
        }
    }
}