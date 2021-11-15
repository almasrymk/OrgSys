using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class StoreService : BaseService<StoreModelView>
    {
        string Includes = "Branch";
        UnitOfWorkOrg repo;
        public StoreService(string Schema)
        {
            repo = new UnitOfWorkOrg(Schema);
        }

        #region Save / Delete
        public StoreModelView Save(StoreModelView ob)
        {
            return new StoreModelView(repo.storeRepo.AddOrUpdate(ob.Model()));
        }

        public bool Delete(long id)
        {
            return repo.storeRepo.Delete(id);
        }

        public bool Delete(List<long> ids)
        {
            return repo.storeRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public StoreModelView Get(long Id)
        {
            return new StoreModelView(repo.storeRepo.Get(e => e.Id == Id, Includes));
        }

        public StoreModelView Get(string textSearch)
        {
            return new StoreModelView(repo.storeRepo.Get(e => e.Name.Contains("" + textSearch), Includes));
        }

        public List<StoreModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.storeRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new StoreModelView(e)).ToList();
        }

        public List<StoreModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0)
        {
            return repo.storeRepo.GetList(e => e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new StoreModelView(e)).ToList();
        }

        public IPagedList<StoreModelView> GetAll(long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.storeRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new StoreModelView(e)).ToPagedList(page, pageSize);
        }

        public IPagedList<StoreModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.storeRepo.GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new StoreModelView(e)).ToPagedList(page, pageSize);
        }

        public List<StoreModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.storeRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new StoreModelView(e)).ToList();
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.storeRepo.GetMaXCode();
        }
        #endregion
    }
}