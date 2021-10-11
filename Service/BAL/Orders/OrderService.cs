using Entity.Model;
using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class OrderService : BaseService<OrderModelView>
    {
        string Includes = "Dealer,Table,Invoice,OrderProducts,OrderProducts.Product,OrderProducts.Product.ProductUnits,,OrderProducts.Product.ProductUnits.Unit";
        UnitOfWork repo;
        public OrderService()
        {
            repo = new UnitOfWork();
        }

        #region Save / Delete
        public OrderModelView Save(OrderModelView ob)
        {
            if (ob.Id > 0)
            {
                ob.InvoiceId = Get(ob.Id).InvoiceId;
            }

            // Save
            var Nwob = repo.orderRepo.AddOrUpdate(ob.Model);

            if (ob.Id > 0)
            {
                var ids = ob.OrderProducts.Select(e => e.Id).ToList();
                if (ids == null) ids = new List<long>();

                // Delete row from database
                var deleted = repo.orderProductRepo.GetList(e => e.OrderId == ob.Id && !ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
                if (deleted != null && deleted.Count > 0)
                    repo.orderProductRepo.ShiftDelete(deleted.Select(e => e.Id).ToList());

                foreach (var productUnit in ob.OrderProducts)
                {
                    var model = productUnit.Model;
                    model.OrderId = Nwob.Id;
                    repo.orderProductRepo.AddOrUpdate(model);
                }
                Nwob.OrderProducts = repo.orderProductRepo.GetList(e => e.OrderId == Nwob.Id, e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
            }

            if (long.Parse("0" + new PreferenceService().GetByKey("AutoCreateInvoice", "Order", ob.TypeId, 0)?.Value) == 1 || ob.InvoiceId > 0)
            {
                new IntegrationServics().CreateInvoiceByOrder(Nwob);
            }

            return new OrderModelView(Nwob);
        }

        public bool Delete(long id)
        {
            var ob = repo.orderRepo.Get(e => e.Id == id);
            if (ob != null)
            {
                repo.orderRepo.Delete(id);
                new InvoiceService().Delete(ob.InvoiceId ?? 0);
                return true;
            }
            return false;
        }

        public bool Delete(List<long> ids)
        {
            var oblist = repo.orderRepo.GetList(e => ids.Contains(e.Id), null, "", Utility.Status.New);
            if (oblist != null && oblist.Count() > 0)
            {
                repo.orderRepo.Delete(ids);
                new InvoiceService().Delete(oblist.Select(e => e.InvoiceId ?? 0).ToList());
                return true;
            }
            return false;
        }
        #endregion

        #region Gets
        public OrderModelView Get(long Id)
        {
            return new OrderModelView(repo.orderRepo.Get(e => e.Id == Id, Includes));
        }

        public OrderModelView Get(string textSearch)
        {
            return new OrderModelView(repo.orderRepo.Get(e => e.Dealer.Name.Contains("" + textSearch), Includes));
        }

        public List<OrderModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.orderRepo.GetList(e => e.TypeId == TypeId, e => e.OrderByDescending(e => e.Id), Includes, Utility.Status.New).Select(e => new OrderModelView(e)).ToList();
        }

        public List<OrderModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0)
        {
            return repo.orderRepo.GetList(e => e.Dealer.Name.Contains("" + textSearch) && e.TypeId == TypeId, e => e.OrderByDescending(e => e.Id), Includes, Utility.Status.New).Select(e => new OrderModelView(e)).ToList();
        }

        public IPagedList<OrderModelView> GetAll(long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.orderRepo.GetList(e => e.TypeId == TypeId, e => e.OrderByDescending(e => e.Id), Includes, Utility.Status.New).Select(e => new OrderModelView(e)).ToPagedList(page, pageSize);
        }

        public IPagedList<OrderModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.orderRepo.GetList(e => e.TypeId == TypeId && ("" + textSearch == "" || e.Dealer.Name.Contains("" + textSearch)), e => e.OrderByDescending(e => e.Id), Includes, Utility.Status.New).Select(e => new OrderModelView(e)).ToPagedList(page, pageSize);
        }

        public List<OrderModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.orderRepo.GetList(e => e.TypeId == TypeId && ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new OrderModelView(e)).ToList();
        }

        public long GetMaxCode(long type)
        {
            return repo.orderRepo.GetMaXCode(e => e.TypeId == type);
        }       
           
        public void Cancel(long Id)
        {
            var ob = repo.orderRepo.Get(e => e.Id == Id);
            ob.Status = Utility.Status.Cancel;
            ob.CloseTable = true;
            ob = repo.orderRepo.AddOrUpdate(ob);
        }

        public void Redo(long Id)
        {
            var ob = repo.orderRepo.Get(e => e.Id == Id);
            ob.Status = Utility.Status.All;
            ob.CloseTable = false;
            ob = repo.orderRepo.AddOrUpdate(ob);
        }
        #endregion
    }
}