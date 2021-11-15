using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class PropertyElementService :BaseService<PropertyElementModelView>
    {
        string Includes = "";
        UnitOfWorkOrg repo;
        public PropertyElementService(string Schema)
        {
            repo = new UnitOfWorkOrg(Schema);
        }

        #region Save / Delete
        public PropertyElementModelView Save(PropertyElementModelView ob)
        {
            return new PropertyElementModelView(repo.propertyelementRepo.AddOrUpdate(ob.Model()));
        }

        public bool Delete(long id)
        {
            return repo.propertyelementRepo.Delete(id);
        }

        public bool Delete(List<long> ids)
        {
            return repo.propertyelementRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public PropertyElementModelView Get(long Id)
        {
            return new PropertyElementModelView(repo.propertyelementRepo.Get(e => e.Id == Id , Includes));
        }

        public PropertyElementModelView Get(string textSearch)
        {
            return new PropertyElementModelView(repo.propertyelementRepo.Get(null , Includes));
        }

        public List<PropertyElementModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.propertyelementRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new PropertyElementModelView(e)).ToList();
        }
        
        public List<PropertyElementModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0)
        {
            return repo.propertyelementRepo.GetList(null, e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new PropertyElementModelView(e)).ToList();
        }
       
        public IPagedList<PropertyElementModelView> GetAll(long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.propertyelementRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new PropertyElementModelView(e)).ToPagedList(page, pageSize);
        }
      
        public IPagedList<PropertyElementModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.propertyelementRepo.GetList(null, e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new PropertyElementModelView(e)).ToPagedList(page, pageSize);
        }                      

        public List<PropertyElementModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.propertyelementRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new PropertyElementModelView(e)).ToList();
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.propertyelementRepo.GetMaXCode();
        }
        #endregion
    }
}