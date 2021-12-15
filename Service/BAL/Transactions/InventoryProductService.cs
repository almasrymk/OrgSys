using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class InventoryProductService : IBaseService<InventoryProductModelView>
    {
        string Includes = "Product";
        UnitOfWorkOrg repo;
        private string _Schema;
        public InventoryProductService(string Schema)
        {
            this._Schema = Schema;
            repo = new UnitOfWorkOrg(Schema);
        }

        #region Save / Delete
        public InventoryProductModelView Save(InventoryProductModelView ob)
        {
            return new InventoryProductModelView(repo.inventoryProductRepo.AddOrUpdate(ob.Model()));
        }

        public bool Delete(long id)
        {
            return repo.inventoryProductRepo.Delete(id);
        }

        public bool Delete(List<long> ids)
        {
            return repo.inventoryProductRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public InventoryProductModelView Get(long Id)
        {
            return new InventoryProductModelView(repo.inventoryProductRepo.Get(e => e.Id == Id, Includes));
        }

        public InventoryProductModelView Get(string textSearch)
        {
            return new InventoryProductModelView(repo.inventoryProductRepo.Get(e => e.Product.Name.Contains("" + textSearch), Includes));
        }

        public List<InventoryProductModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.inventoryProductRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new InventoryProductModelView(e)).ToList();
        }
         
        public List<InventoryProductModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.inventoryProductRepo.GetList(e => e.Product.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new InventoryProductModelView(e)).ToList();
        }
         
        public IPagedList<InventoryProductModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.inventoryProductRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new InventoryProductModelView(e)).ToPagedList(page, pageSize);
        }
         
        public IPagedList<InventoryProductModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.inventoryProductRepo.GetList(e => "" + textSearch == "" || e.Product.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new InventoryProductModelView(e)).ToPagedList(page, pageSize);
        }
                
        public List<InventoryProductModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.inventoryProductRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id) , Includes, Utility.Status.New).Select(e => new InventoryProductModelView(e)).ToList();
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.inventoryProductRepo.GetMaXCode();
        }
        #endregion
    }
}