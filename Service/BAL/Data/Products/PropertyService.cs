using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class PropertyService : IBaseService<PropertyModelView>
    {
        string Includes = "";
        UnitOfWorkOrg repo;
        public PropertyService(string Schema)
        {
            repo = new UnitOfWorkOrg(Schema);
        }

        #region Save / Delete
        public PropertyModelView Save(PropertyModelView ob)
        {    // Save
            var Nwob = repo.propertyRepo.AddOrUpdate(ob.Model());
            var ids = ob.PropertyElements.Select(e => e.Id).ToList();
            if (ids == null) ids = new List<long>();

            // Delete row from database
            var deleted = repo.propertyelementRepo.GetList(e => e.PropertyId == ob.Id && !ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
            if (deleted != null && deleted.Count > 0)
                repo.propertyelementRepo.ShiftDelete(deleted.Select(e => e.Id).ToList());
            foreach (var propertyelement in ob.PropertyElements)
            {
                var model = propertyelement.Model();
                model.PropertyId = Nwob.Id;
                repo.propertyelementRepo.AddOrUpdate(model);
            }
            Nwob.propertyElements = repo.propertyelementRepo.GetList(e => e.PropertyId == Nwob.Id, e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
            return new PropertyModelView(Nwob);
        }
         
        public bool Delete(long id)
        {
            return repo.propertyRepo.Delete(id);
        }

        public bool Delete(List<long> ids)
        {
            return repo.propertyRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public PropertyModelView Get(long Id)
        {
            var ob = repo.propertyRepo.Get(e => e.Id == Id , Includes);
            if (ob != null)
            {
                ob.propertyElements = repo.propertyelementRepo.GetList(e => e.PropertyId == Id, e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
            }
            return new PropertyModelView(ob);
        }

        public PropertyModelView Get(string textSearch)
        {
            var ob = repo.propertyRepo.Get(e => e.Name.Contains(textSearch) || "" + textSearch == "" , Includes);
            if (ob != null)
            {
                ob.propertyElements = repo.propertyelementRepo.GetList(e => e.PropertyId == ob.Id, e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
            }

            return new PropertyModelView(ob);
        }

        public List<PropertyModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.propertyRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new PropertyModelView(e)).ToList();
        }
         
        public List<PropertyModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0)
        {
            return repo.propertyRepo.GetList(e => e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new PropertyModelView(e)).ToList();
        }
         
        public IPagedList<PropertyModelView> GetAll(long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.propertyRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new PropertyModelView(e)).ToPagedList(page, pageSize);
        }
         
        public IPagedList<PropertyModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.propertyRepo.GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new PropertyModelView(e)).ToPagedList(page, pageSize);
        }
        
        public List<PropertyModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.propertyRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new PropertyModelView(e)).ToList();
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.propertyRepo.GetMaXCode();
        }
        #endregion
    }
}