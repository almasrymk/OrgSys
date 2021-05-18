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
   public class PropertyElementService :BaseService<PropertyElementModelView>
    {
        UnitOfWork repo;
        public PropertyElementService()
        {
            repo = new UnitOfWork();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public PropertyElementModelView Save(PropertyElementModelView ob)
        {
            return new PropertyElementModelView(repo.propertyelementRepo.AddOrUpdate(ob.Model));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(long id)
        {
            return repo.propertyelementRepo.Delete(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<PropertyElementModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.propertyelementRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new PropertyElementModelView(e)).ToList();
        }
        //public List<ProductUnitModelView> GetByProductId(long productId = 0)
        //{
        //    return repo.productUnitRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Where(e=>e.ProductId==productId).Select(e => new ProductUnitModelView(e)).ToList();
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<PropertyElementModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0)
        {
            return repo.propertyelementRepo.GetList(null, e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new PropertyElementModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<PropertyElementModelView> GetAll(long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.propertyelementRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new PropertyElementModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<PropertyElementModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.propertyelementRepo.GetList(null, e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new PropertyElementModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PropertyElementModelView Get(long Id)
        {
            return new PropertyElementModelView(repo.propertyelementRepo.Get(e => e.Id == Id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public PropertyElementModelView Get(string textSearch)
        {
            return new PropertyElementModelView(repo.propertyelementRepo.Get(null));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(List<long> ids)
        {
            return repo.propertyelementRepo.Delete(ids);
        }

        public List<PropertyElementModelView> GetAll(List<long> ids)
        {
            return repo.propertyelementRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new PropertyElementModelView(e)).ToList();
        }
    }
}
