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
    public class InventoryProductService : BaseService<InventoryProductModelView>
    {
        UnitOfWork repo;
        public InventoryProductService()
        {
            repo = new UnitOfWork();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public InventoryProductModelView Save(InventoryProductModelView ob)
        {
            return new InventoryProductModelView(repo.inventoryProductRepo.AddOrUpdate(ob.Model));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(long id)
        {
            return repo.inventoryProductRepo.Delete(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<InventoryProductModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.inventoryProductRepo.GetList(e => e.OrderBy(e => e.Id), "Product", Utility.Status.New).Select(e => new InventoryProductModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<InventoryProductModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.inventoryProductRepo.GetList(e => e.Product.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "Product", Utility.Status.New).Select(e => new InventoryProductModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<InventoryProductModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.inventoryProductRepo.GetList(e => e.OrderBy(e => e.Id), "Product", Utility.Status.New).Select(e => new InventoryProductModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<InventoryProductModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.inventoryProductRepo.GetList(e => "" + textSearch == "" || e.Product.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "Product", Utility.Status.New).Select(e => new InventoryProductModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InventoryProductModelView Get(long Id)
        {
            return new InventoryProductModelView(repo.inventoryProductRepo.Get(e => e.Id == Id  , "Product"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public InventoryProductModelView Get(string textSearch)
        {
            return new InventoryProductModelView(repo.inventoryProductRepo.Get(e => e.Product.Name.Contains("" + textSearch) , "Product"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(List<long> ids)
        {
            return repo.inventoryProductRepo.Delete(ids);
        }

        public List<InventoryProductModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.inventoryProductRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id) , "Product", Utility.Status.New).Select(e => new InventoryProductModelView(e)).ToList();
        }
    }
}