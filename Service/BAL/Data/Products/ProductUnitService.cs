using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class ProductUnitService : BaseService<ProductUnitModelView>
    {
        string Includes = "";
        UnitOfWork repo;
        public ProductUnitService()
        {
            repo = new UnitOfWork();
        }

        #region Save / Delete
        public ProductUnitModelView Save(ProductUnitModelView ob)
        {
            return new ProductUnitModelView(repo.productUnitRepo.AddOrUpdate(ob.Model()));
        }
       
        public bool Delete(long id)
        {
            return repo.productUnitRepo.Delete(id);
        }

        public bool Delete(List<long> ids)
        {
            return repo.productUnitRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public ProductUnitModelView Get(long Id)
        {
            return new ProductUnitModelView(repo.productUnitRepo.Get(e => e.Id == Id , Includes));
        }

        public ProductUnitModelView Get(string textSearch)
        {
            return new ProductUnitModelView(repo.productUnitRepo.Get(null , Includes));
        }

        public List<ProductUnitModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.productUnitRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new ProductUnitModelView(e)).ToList();
        }
       
        public List<ProductUnitModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.productUnitRepo.GetList( null , e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new ProductUnitModelView(e)).ToList();
        }

        public IPagedList<ProductUnitModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.productUnitRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new ProductUnitModelView(e)).ToPagedList(page, pageSize);
        }
       
        public IPagedList<ProductUnitModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.productUnitRepo.GetList(null, e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new ProductUnitModelView(e)).ToPagedList(page, pageSize);
        }
       
        public List<ProductUnitModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.productUnitRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new ProductUnitModelView(e)).ToList();
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.productUnitRepo.GetMaXCode();
        }
        #endregion
    }
}