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
   public class ProductPropertyElementService : BaseService<ProductPropertyElementModelView>
    {
        UnitOfWork repo;
        public ProductPropertyElementService()
        {
            repo = new UnitOfWork();
        }
        public ProductPropertyElementModelView Save(ProductPropertyElementModelView ob)
        {
            return new ProductPropertyElementModelView(repo.productpropertyelementRepo.AddOrUpdate(ob.Model));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(long id)
        {
            return repo.productpropertyelementRepo.Delete(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ProductPropertyElementModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.productpropertyelementRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new ProductPropertyElementModelView(e)).ToList();
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
        public List<ProductPropertyElementModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0)
        {
            return repo.productpropertyelementRepo.GetList(null, e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new ProductPropertyElementModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<ProductPropertyElementModelView> GetAll(long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.productpropertyelementRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new ProductPropertyElementModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<ProductPropertyElementModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.productpropertyelementRepo.GetList(null, e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new ProductPropertyElementModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProductPropertyElementModelView Get(long Id)
        {
            return new ProductPropertyElementModelView(repo.productpropertyelementRepo.Get(e => e.Id == Id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public ProductPropertyElementModelView Get(string textSearch)
        {
            return new ProductPropertyElementModelView(repo.productpropertyelementRepo.Get(null));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(List<long> ids)
        {
            return repo.productpropertyelementRepo.Delete(ids);
        }

        public List<ProductPropertyElementModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.productpropertyelementRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new ProductPropertyElementModelView(e)).ToList();
        }
    }
}
