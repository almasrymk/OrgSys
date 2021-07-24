using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class ProductPropertyElementService : BaseService<ProductPropertyElementModelView>
    {
        string Includes = "";
        UnitOfWork repo;
        public ProductPropertyElementService()
        {
            repo = new UnitOfWork();
        }

        #region Save / Delete
        public ProductPropertyElementModelView Save(ProductPropertyElementModelView ob)
        {
            return new ProductPropertyElementModelView(repo.productpropertyelementRepo.AddOrUpdate(ob.Model));
        }
 
        public bool Delete(long id)
        {
            return repo.productpropertyelementRepo.Delete(id);
        }

        public bool Delete(List<long> ids)
        {
            return repo.productpropertyelementRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public ProductPropertyElementModelView Get(long Id)
        {
            return new ProductPropertyElementModelView(repo.productpropertyelementRepo.Get(e => e.Id == Id , Includes));
        }

        public ProductPropertyElementModelView Get(string textSearch)
        {
            return new ProductPropertyElementModelView(repo.productpropertyelementRepo.Get(null , Includes));
        }

        public List<ProductPropertyElementModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.productpropertyelementRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new ProductPropertyElementModelView(e)).ToList();
        }
       
        public List<ProductPropertyElementModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0)
        {
            return repo.productpropertyelementRepo.GetList(null, e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new ProductPropertyElementModelView(e)).ToList();
        }
      
        public IPagedList<ProductPropertyElementModelView> GetAll(long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.productpropertyelementRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new ProductPropertyElementModelView(e)).ToPagedList(page, pageSize);
        }
       
        public IPagedList<ProductPropertyElementModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.productpropertyelementRepo.GetList(null, e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new ProductPropertyElementModelView(e)).ToPagedList(page, pageSize);
        }              
         
        public List<ProductPropertyElementModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.productpropertyelementRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new ProductPropertyElementModelView(e)).ToList();
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.productpropertyelementRepo.GetMaXCode();
        }
        #endregion
    }
}