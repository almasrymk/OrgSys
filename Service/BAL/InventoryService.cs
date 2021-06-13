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
    public class InventoryService : BaseService<InventoryModelView>
    {
        UnitOfWork repo;
        public InventoryService()
        {
            repo = new UnitOfWork();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public InventoryModelView Save(InventoryModelView ob)
        {
            // Save
            var Nwob = repo.inventoryRepo.AddOrUpdate(ob.Model);

            //if (ob.Id > 0)
            //{
            //    var ids = ob.InventoryProducts.Select(e => e.Id).ToList();
            //    if (ids == null) ids = new List<long>();

            //    // Delete row from database
            //    var deleted = repo.InventoryProductRepo.GetList(e => e.InventoryId == ob.Id && !ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
            //    if (deleted != null && deleted.Count > 0)
            //        repo.InventoryProductRepo.ShiftDelete(deleted.Select(e => e.Id).ToList());

            //    foreach (var productUnit in ob.InventoryProducts)
            //    {
            //        var model = productUnit.Model;
            //        model.InventoryId = Nwob.Id;
            //        repo.InventoryProductRepo.AddOrUpdate(model);
            //    }
            //    Nwob.InventoryProducts = repo.InventoryProductRepo.GetList(e => e.InventoryId == Nwob.Id, e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
            //}
            return new InventoryModelView(Nwob);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(long id)
        {
            return repo.inventoryRepo.Delete(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<InventoryModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.inventoryRepo.GetList( e=>e.TypeId == TypeId, e => e.OrderByDescending(e => e.Id), "", Utility.Status.New).Select(e => new InventoryModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<InventoryModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.inventoryRepo.GetList(e => e.Code.Contains("" + textSearch), e => e.OrderByDescending(e => e.Id), "Dealer", Utility.Status.New).Select(e => new InventoryModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<InventoryModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.inventoryRepo.GetList(null, e => e.OrderByDescending(e => e.Id), "", Utility.Status.New).Select(e => new InventoryModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<InventoryModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.inventoryRepo.GetList(null, e => e.OrderByDescending(e => e.Id), "", Utility.Status.New).Select(e => new InventoryModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InventoryModelView Get(long Id)
        {
            return new InventoryModelView(repo.inventoryRepo.Get(e => e.Id == Id  , ""));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public InventoryModelView Get(string textSearch)
        {
            return new InventoryModelView(repo.inventoryRepo.Get(null , ""));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(List<long> ids)
        {
            return repo.inventoryRepo.Delete(ids);
        }

        public List<InventoryModelView> GetAll(List<long> ids,long TypeId = 0)
        {
            return repo.inventoryRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id) , "", Utility.Status.New).Select(e => new InventoryModelView(e)).ToList();
        }
       // , long TypeId
        public long GetMaxCode(long type )
        {
            return repo.inventoryRepo.GetMaXCode(type);
        }
    }
}