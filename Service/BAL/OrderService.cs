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
    public class OrderService : BaseService<OrderModelView>
    {
        UnitOfWork repo;
        public OrderService()
        {
            repo = new UnitOfWork();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public OrderModelView Save(OrderModelView ob)
        {
            if(ob.Id > 0)
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

            var setting = new PreferenceService();
            if (long.Parse("0" + setting.GetByKey("AutoCreateInvoice", "Order", ob.TypeId, 0)?.Value) == 1 || ob.InvoiceId > 0)
            {
                Nwob = CreateInvoice(Nwob);
            }

            return new OrderModelView(Nwob);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Order CreateInvoice(long Id)
        {
            var ob = Get(Id);
            return CreateInvoice(ob.Model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public Order CreateInvoice(Order ob)
        {           
            if(ob != null)
            {
                var currencyId = repo.currencyRepo.Get(e => e.IsDefault)?.Id??0;
                if (currencyId > 0)
                {
                    var inv = repo.invoiceRepo.Get(e => e.Id == ob.InvoiceId);
                    if (inv == null || inv.Id == 0)
                    {
                        inv = new Invoice();
                        var setting = new PreferenceService();
                        inv.StoreId = long.Parse("0" + setting.GetByKey("DefaultStore", "Invoice", 1, 0)?.Value);
                        if (ob.DealerId == null || ob.DealerId == 0)
                            inv.DealerId = long.Parse("0" + setting.GetByKey("DefaultCustomer", "Invoice", 1, 0)?.Value);
                        else
                            inv.DealerId = ob.DealerId.Value;
                        inv.PaymentTypeId = long.Parse("0" + setting.GetByKey("DefaultPaymentType", "Invoice", 1, 0)?.Value);
                        inv.TypeId = 1;
                        inv.CodeNumber = new InvoiceService().GetMaxCode(1);
                        inv.Code = "" + new InvoiceService().GetMaxCode(1);
                    }
                    var obInv = new InvoiceService().Save(new InvoiceModelView(inv).UpdateData(ob, inv.StoreId, currencyId));
                    ob.InvoiceId = obInv.Id;
                    ob.CloseTable = true;
                    ob = repo.orderRepo.AddOrUpdate(ob);
                }
            }
            return ob;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        public void Cancel(long Id)
        {
            var ob = repo.orderRepo.Get(e => e.Id == Id);
            ob.Status = Utility.Status.Cancel;
            ob.CloseTable = true;
            ob = repo.orderRepo.AddOrUpdate(ob);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        public void Redo(long Id)
        {
            var ob = repo.orderRepo.Get(e => e.Id == Id);
            ob.Status = Utility.Status.All;
            ob.CloseTable = false;
            ob = repo.orderRepo.AddOrUpdate(ob);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(long id)
        {
            var ob = repo.orderRepo.Get(e=>e.Id == id);
            if(ob != null)
            {
                repo.orderRepo.Delete(id);
                new InvoiceService().Delete(ob.InvoiceId ?? 0);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<OrderModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.orderRepo.GetList( e=>e.TypeId == TypeId, e => e.OrderByDescending(e => e.Id), "Dealer,Table,Invoice", Utility.Status.New).Select(e => new OrderModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<OrderModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.orderRepo.GetList(e => e.Dealer.Name.Contains("" + textSearch) && e.TypeId == TypeId, e => e.OrderByDescending(e => e.Id), "Dealer,Table,Invoice", Utility.Status.New).Select(e => new OrderModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<OrderModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.orderRepo.GetList(e=> e.TypeId == TypeId , e => e.OrderByDescending(e => e.Id), "Dealer,Table,Invoice", Utility.Status.New).Select(e => new OrderModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<OrderModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.orderRepo.GetList(e => e.TypeId == TypeId && ("" + textSearch == "" || e.Dealer.Name.Contains("" + textSearch)), e => e.OrderByDescending(e => e.Id), "Dealer,Table,Invoice", Utility.Status.New).Select(e => new OrderModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OrderModelView Get(long Id)
        {
            return new OrderModelView(repo.orderRepo.Get(e => e.Id == Id  , "Dealer,Table,Invoice,OrderProducts,OrderProducts.Product,OrderProducts.Product.ProductUnits,,OrderProducts.Product.ProductUnits.Unit"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public OrderModelView Get(string textSearch)
        {
            return new OrderModelView(repo.orderRepo.Get(e => e.Dealer.Name.Contains("" + textSearch) , "Dealer,Table,Invoice"));
        }       

        public List<OrderModelView> GetAll(List<long> ids,long TypeId = 0)
        {
            return repo.orderRepo.GetList(e => e.TypeId == TypeId && ids.Contains(e.Id), e => e.OrderBy(e => e.Id) , "Dealer,Table,Invoice", Utility.Status.New).Select(e => new OrderModelView(e)).ToList();
        }
       // , long TypeId
        public long GetMaxCode(long type )
        {
            return repo.orderRepo .GetMaXCode(type);
        }
    }
}