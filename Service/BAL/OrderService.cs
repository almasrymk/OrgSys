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
            return new OrderModelView(Nwob);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(long id)
        {
            return repo.orderRepo.Delete(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<OrderModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.orderRepo.GetList( e=>e.TypeId == TypeId, e => e.OrderByDescending(e => e.Id), "Dealer", Utility.Status.New).Select(e => new OrderModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<OrderModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.orderRepo.GetList(e => e.Dealer.Name.Contains("" + textSearch) && e.TypeId == TypeId, e => e.OrderByDescending(e => e.Id), "Dealer", Utility.Status.New).Select(e => new OrderModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<OrderModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.orderRepo.GetList(e=> e.TypeId == TypeId , e => e.OrderByDescending(e => e.Id), "Dealer", Utility.Status.New).Select(e => new OrderModelView(e)).ToPagedList(page, pageSize);
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
            return repo.orderRepo.GetList(e => e.TypeId == TypeId && ("" + textSearch == "" || e.Dealer.Name.Contains("" + textSearch)), e => e.OrderByDescending(e => e.Id), "Dealer", Utility.Status.New).Select(e => new OrderModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OrderModelView Get(long Id)
        {
            return new OrderModelView(repo.orderRepo.Get(e => e.Id == Id  , "Dealer,OrderProducts,OrderProducts.Product,OrderProducts.Product.ProductUnits,,OrderProducts.Product.ProductUnits.Unit"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public OrderModelView Get(string textSearch)
        {
            return new OrderModelView(repo.orderRepo.Get(e => e.Dealer.Name.Contains("" + textSearch) , "Dealer"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(List<long> ids)
        {
            return repo.orderRepo.Delete(ids);
        }

        public List<OrderModelView> GetAll(List<long> ids,long TypeId = 0)
        {
            return repo.orderRepo.GetList(e => e.TypeId == TypeId && ids.Contains(e.Id), e => e.OrderBy(e => e.Id) , "Dealer", Utility.Status.New).Select(e => new OrderModelView(e)).ToList();
        }
       // , long TypeId
        public long GetMaxCode(long type )
        {
            return repo.orderRepo .GetMaXCode(type);
        }
    }
}