using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class OutlayService : IBaseService<OutlayModelView>
    {
        string Includes = "";
        UnitOfWorkOrg repo;
        public OutlayService(string Schema)
        {
            repo = new UnitOfWorkOrg(Schema);
        }

        #region Save / Delete
        public OutlayModelView Save(OutlayModelView ob)
        {
            return new OutlayModelView(repo.outlayRepo.AddOrUpdate(ob.Model()));
        }
       
        public bool Delete(long id)
        {
            return repo.outlayRepo.Delete(id);
        }

        public bool Delete(List<long> ids)
        {
            return repo.outlayRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public OutlayModelView Get(long Id)
        {
            return new OutlayModelView(repo.outlayRepo.Get(e => e.Id == Id , Includes));
        }

        public OutlayModelView Get(string textSearch)
        {
            return new OutlayModelView(repo.outlayRepo.Get(e => e.Name.Contains("" + textSearch) , Includes));
        }

        public List<OutlayModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.outlayRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new OutlayModelView(e)).ToList();
        }

        public List<OutlayModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.outlayRepo.GetList(e => e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new OutlayModelView(e)).ToList();
        }

        public IPagedList<OutlayModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.outlayRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new OutlayModelView(e)).ToPagedList(page, pageSize);
        }

        public IPagedList<OutlayModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.outlayRepo.GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new OutlayModelView(e)).ToPagedList(page, pageSize);
        }       

        public List<OutlayModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.outlayRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new OutlayModelView(e)).ToList();
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.outlayRepo.GetMaXCode();
        }
        #endregion
    }
}