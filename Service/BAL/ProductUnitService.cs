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
    public class ProductUnitService : BaseService<ProductUnitModelView>
    {
        UnitOfWork repo;
        public ProductUnitService()
        {
            repo = new UnitOfWork();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public ProductUnitModelView Save(ProductUnitModelView ob)
        {
            return new ProductUnitModelView(repo.productUnitRepo.AddOrUpdate(ob.Model));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(long id)
        {
            return repo.productUnitRepo.Delete(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ProductUnitModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.productUnitRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new ProductUnitModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<ProductUnitModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.productUnitRepo.GetList( null , e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new ProductUnitModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<ProductUnitModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.productUnitRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new ProductUnitModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<ProductUnitModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.productUnitRepo.GetList(null, e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new ProductUnitModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProductUnitModelView Get(long Id)
        {
            return new ProductUnitModelView(repo.productUnitRepo.Get(e => e.Id == Id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public ProductUnitModelView Get(string textSearch)
        {
            return new ProductUnitModelView(repo.productUnitRepo.Get(null));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(List<long> ids)
        {
            return repo.productUnitRepo.Delete(ids);
        }

        public List<ProductUnitModelView> GetAll(List<long> ids)
        {
            return repo.productUnitRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new ProductUnitModelView(e)).ToList();
        }
    }
}