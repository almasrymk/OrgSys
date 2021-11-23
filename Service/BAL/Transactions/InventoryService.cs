using Entity.Model;
using Entity.ModelView;
using X.PagedList;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service
{
    public class InventoryService : BaseService<InventoryModelView>
    {
        string Includes = "Store,InventoryProducts,InventoryProducts.Product,InventoryProducts.Unit";
        UnitOfWorkOrg repo;
        private string _Schema;
        public InventoryService(string Schema)
        {
            this._Schema = Schema;
            repo = new UnitOfWorkOrg(Schema);
        }

        #region Save / Delete
        public InventoryModelView Save(InventoryModelView ob)
        {
            // Save
            var Nwob = repo.inventoryRepo.AddOrUpdate(ob.Model());

            if (ob.Id > 0)
            {
                var ids = ob.InventoryProducts.Select(e => e.Id).ToList();
                if (ids == null) ids = new List<long>();

                // Delete row from database
                var deleted = repo.inventoryProductRepo.GetList(e => e.InventoryId == ob.Id && !ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
                if (deleted != null && deleted.Count > 0)
                    repo.inventoryProductRepo.ShiftDelete(deleted.Select(e => e.Id).ToList());

                foreach (var productUnit in ob.InventoryProducts)
                {
                    var model = productUnit.Model();
                    model.InventoryId = Nwob.Id;
                    repo.inventoryProductRepo.AddOrUpdate(model);
                }
                Nwob.InventoryProducts = repo.inventoryProductRepo.GetList(e => e.InventoryId == Nwob.Id, e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
            }

            CreateTransaction(Nwob, 5);
            CreateTransaction(Nwob, 6);

            return new InventoryModelView(Nwob);
        }       
         
        public bool Delete(long id)
        {
            return repo.inventoryRepo.Delete(id);
        }

        public bool Delete(List<long> ids)
        {
            return repo.inventoryRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public InventoryModelView Get(long Id)
        {
            return new InventoryModelView(repo.inventoryRepo.Get(e => e.Id == Id, Includes));
        }

        public InventoryModelView Get(string textSearch)
        {
            return new InventoryModelView(repo.inventoryRepo.Get(null, Includes));
        }

        public List<InventoryModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.inventoryRepo.GetList(e => e.TypeId == TypeId, e => e.OrderByDescending(e => e.Id), Includes, Utility.Status.New).Select(e => new InventoryModelView(e)).ToList();
        }
         
        public List<InventoryModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0)
        {
            return repo.inventoryRepo.GetList(e => e.Code.Contains("" + textSearch), e => e.OrderByDescending(e => e.Id), Includes, Utility.Status.New).Select(e => new InventoryModelView(e)).ToList();
        }
         
        public IPagedList<InventoryModelView> GetAll(long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.inventoryRepo.GetList(null, e => e.OrderByDescending(e => e.Id), Includes, Utility.Status.New).Select(e => new InventoryModelView(e)).ToPagedList(page, pageSize);
        }
         
        public IPagedList<InventoryModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.inventoryRepo.GetList(null, e => e.OrderByDescending(e => e.Id), Includes, Utility.Status.New).Select(e => new InventoryModelView(e)).ToPagedList(page, pageSize);
        }                
         
        public List<InventoryModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.inventoryRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new InventoryModelView(e)).ToList();
        }
         
        public long GetMaxCode(long type)
        {
            return repo.inventoryRepo.GetMaXCode(e=>e.TypeId == type);
        }
        #endregion

        #region Integration
        public void CreateTransaction(long Id)
        {
            var ob = Get(Id);
            CreateTransaction(ob.Model(), 5);
            CreateTransaction(ob.Model(), 6);
        }

        public void CreateTransaction(Inventory ob, long typeId)
        {
            if (ob != null)
            {
                var trns = repo.transactionRepo.Get(e => e.ParentId == ob.Id && e.TypeId == typeId);
                if (trns == null || trns.Id == 0)
                {
                    trns = new Transaction();
                    trns.StoreId = ob.StoreId;
                    trns.TypeId = typeId;
                    trns.ParentId = ob.Id;
                    trns.CodeNumber = new TransactionService(_Schema).GetMaxCode(typeId);
                    trns.Code = "" + new TransactionService(_Schema).GetMaxCode(typeId);
                }
                //var obInv = new TransactionService().Save(new TransactionModelView(trns).UpdateData(ob, typeId));
            }
        }
        #endregion
    }
}