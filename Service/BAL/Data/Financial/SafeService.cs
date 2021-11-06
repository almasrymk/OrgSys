using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class SafeService : BaseService<SafeModelView>
    {
        string Includes = "";
        UnitOfWork repo;
        public SafeService()
        {
            if (repo == null)
                repo = new UnitOfWork();
        }

        #region Save / Delete
        public SafeModelView Save(SafeModelView ob)
        {
            return new SafeModelView(repo.safeRepo.AddOrUpdate(ob.Model()));
        }

        public bool Delete(long id)
        {
            return repo.safeRepo.Delete(id);
        }

        public bool Delete(List<long> ids)
        {
            return repo.safeRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public SafeModelView Get(long Id)
        {
            return new SafeModelView(repo.safeRepo.Get(e => e.Id == Id, Includes));
        }

        public SafeModelView Get(string textSearch)
        {
            return new SafeModelView(repo.safeRepo.Get(e => e.Name.Contains("" + textSearch), Includes));
        }

        public List<SafeModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.safeRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new SafeModelView(e)).ToList();
        }

        public List<SafeModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0)
        {
            return repo.safeRepo.GetList(e => e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new SafeModelView(e)).ToList();
        }

        public IPagedList<SafeModelView> GetAll(long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.safeRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new SafeModelView(e)).ToPagedList(page, pageSize);
        }

        public IPagedList<SafeModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.safeRepo.GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new SafeModelView(e)).ToPagedList(page, pageSize);
        }

        public List<SafeModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.safeRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new SafeModelView(e)).ToList();
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.safeRepo.GetMaXCode();
        }
        #endregion
    }
}