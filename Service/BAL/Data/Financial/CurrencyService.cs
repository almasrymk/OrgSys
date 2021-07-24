using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class CurrencyService : BaseService<CurrencyModelView>
    {
        string Includes = "";
        UnitOfWork repo;
        public CurrencyService()
        {
            if (repo == null)
                repo = new UnitOfWork();
        }

        #region Save / Delete
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
     
        public bool Delete(long id)
        {
            return repo.currencyRepo.Delete(id);
        }

        public bool Delete(List<long> ids)
        {
            return repo.currencyRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public CurrencyModelView Get(long Id)
        {
            return new CurrencyModelView(repo.currencyRepo.Get(e => e.Id == Id, Includes));
        }

        public CurrencyModelView Get(string textSearch)
        {
            return new CurrencyModelView(repo.currencyRepo.Get(e => e.Name.Contains("" + textSearch), Includes));
        }

        public List<CurrencyModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.currencyRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new CurrencyModelView(e)).ToList();
        }
      
        public List<CurrencyModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.currencyRepo.GetList(e => e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new CurrencyModelView(e)).ToList();
        }
       
        public IPagedList<CurrencyModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.currencyRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new CurrencyModelView(e)).ToPagedList(page, pageSize);
        }
       
        public IPagedList<CurrencyModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.currencyRepo.GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new CurrencyModelView(e)).ToPagedList(page, pageSize);
        }
       
        public List<CurrencyModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.currencyRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new CurrencyModelView(e)).ToList();
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.currencyRepo.GetMaXCode();
        }
        #endregion
    }
}