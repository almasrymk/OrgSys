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
   public class PropertyService : BaseService<PropertyModelView>
    {
        UnitOfWork repo;
        public PropertyService()
        {
            repo = new UnitOfWork();
        }
        public PropertyModelView Save(PropertyModelView ob)
        {    // Save
            var Nwob = repo.propertyRepo.AddOrUpdate(ob.Model);
            var ids = ob.PropertyElements.Select(e => e.Id).ToList();
            if (ids == null) ids = new List<long>();

            // Delete row from database
            var deleted = repo.propertyelementRepo.GetList(e => e.PropertyId == ob.Id && !ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
            if (deleted != null && deleted.Count > 0)
                repo.propertyelementRepo.ShiftDelete(deleted.Select(e => e.Id).ToList());
            foreach (var propertyelement in ob.PropertyElements)
            {
                var model = propertyelement.Model;
                model.PropertyId = Nwob.Id;
                repo.propertyelementRepo.AddOrUpdate(model);
            }
            Nwob.propertyElements = repo.propertyelementRepo.GetList(e => e.PropertyId == Nwob.Id, e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
            return new PropertyModelView(Nwob);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(long id)
        {
            return repo.propertyRepo.Delete(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<PropertyModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.propertyRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new PropertyModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<PropertyModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0)
        {
            return repo.propertyRepo.GetList(e => e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new PropertyModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<PropertyModelView> GetAll(long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.propertyRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new PropertyModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<PropertyModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.propertyRepo.GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new PropertyModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PropertyModelView Get(long Id)
        {
            var ob = repo.propertyRepo.Get(e => e.Id == Id);
            if (ob != null)
            {
                ob.propertyElements = repo.propertyelementRepo.GetList(e => e.PropertyId == Id, e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
            }
            return new PropertyModelView(ob);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public PropertyModelView Get(string textSearch)
        {
            var ob = repo.propertyRepo.Get(e => e.Name.Contains(textSearch)|| "" + textSearch == "");
            if (ob != null)
            {
                ob.propertyElements = repo.propertyelementRepo.GetList(e => e.PropertyId == ob.Id, e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
            }

            return new PropertyModelView(ob);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(List<long> ids)
        {
            return repo.propertyRepo.Delete(ids);
        }

        public List<PropertyModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.propertyRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new PropertyModelView(e)).ToList();
        }
    }
}
