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
    public class OrderProductService : BaseService<OrderProductModelView>
    {
        UnitOfWork repo;
        public OrderProductService()
        {
            repo = new UnitOfWork();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public OrderProductModelView Save(OrderProductModelView ob)
        {
            return new OrderProductModelView(repo.orderProductRepo.AddOrUpdate(ob.Model));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(long id)
        {
            return repo.orderProductRepo.Delete(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<OrderProductModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.orderProductRepo.GetList(e => e.OrderBy(e => e.Id), "Product", Utility.Status.New).Select(e => new OrderProductModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<OrderProductModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.orderProductRepo.GetList(e => e.Product.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "Product", Utility.Status.New).Select(e => new OrderProductModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<OrderProductModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.orderProductRepo.GetList(e => e.OrderBy(e => e.Id), "Product", Utility.Status.New).Select(e => new OrderProductModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<OrderProductModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.orderProductRepo.GetList(e => "" + textSearch == "" || e.Product.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "Product", Utility.Status.New).Select(e => new OrderProductModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OrderProductModelView Get(long Id)
        {
            return new OrderProductModelView(repo.orderProductRepo.Get(e => e.Id == Id  , "Product"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public OrderProductModelView Get(string textSearch)
        {
            return new OrderProductModelView(repo.orderProductRepo.Get(e => e.Product.Name.Contains("" + textSearch) , "Product"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(List<long> ids)
        {
            return repo.orderProductRepo.Delete(ids);
        }

        public List<OrderProductModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.orderProductRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id) , "Product", Utility.Status.New).Select(e => new OrderProductModelView(e)).ToList();
        }
    }
}