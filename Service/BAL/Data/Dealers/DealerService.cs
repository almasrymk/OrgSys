using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class DealerService : IBaseService<DealerModelView>
    {
        string Includes = "";
        UnitOfWorkOrg repo;

        public DealerService(string Schema)
        {
            repo = new UnitOfWorkOrg(Schema);
        }

        #region Save / Delete
        public DealerModelView Save(DealerModelView ob)
        {
            return new DealerModelView(repo.dealerRepo.AddOrUpdate(ob.Model()));
        }

        public bool Delete(long id)
        {
            return repo.dealerRepo.Delete(id);           
        }

        public bool Delete(List<long> ids)
        {
            return repo.dealerRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public DealerModelView Get(long Id)
        {
            return new DealerModelView(repo.dealerRepo.Get(e => e.Id == Id, Includes));
        }

        public DealerModelView Get(string textSearch)
        {
            return new DealerModelView(repo.dealerRepo.Get(e => e.Name.Contains("" + textSearch), Includes));
        }
        
        public List<DealerModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.dealerRepo.GetList(e=>e.TypeId == TypeId || e.TypeId == 0, e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new DealerModelView(e)).ToList();
        }

        public List<DealerModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.dealerRepo.GetList(e => (e.TypeId == TypeId || e.TypeId == 0) && e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new DealerModelView(e)).ToList();
        }

        public IPagedList<DealerModelView> GetAll(long parentId = 0, long TypeId = 0 , int page = 1, int pageSize = 20)
        {
            return repo.dealerRepo.GetList(e=> e.TypeId == TypeId || e.TypeId == 0, e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new DealerModelView(e)).ToPagedList(page, pageSize);
        }

        public IPagedList<DealerModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.dealerRepo.GetList(e => (e.TypeId == TypeId || e.TypeId == 0) &&( "" + textSearch == "" || e.Name.Contains("" + textSearch)), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new DealerModelView(e)).ToPagedList(page, pageSize);
        }

        public List<DealerModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.dealerRepo.GetList(e => e.TypeId == TypeId && ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new DealerModelView(e)).ToList();
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.dealerRepo.GetMaXCode(e=>e.TypeId == type);
        }
        #endregion
    }
}