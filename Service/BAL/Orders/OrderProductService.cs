using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class OrderProductService : IBaseService<OrderProductModelView>
    {
        string Includes = "Product";
        UnitOfWorkOrg repo;
        private string _Schema;
        public OrderProductService(string Schema)
        {
            this._Schema = Schema;
            repo = new UnitOfWorkOrg(Schema);
        }

        #region Save / Delete
        public OrderProductModelView Save(OrderProductModelView ob)
        {
            return new OrderProductModelView(repo.orderProductRepo.AddOrUpdate(ob.Model()));
        }
         
        public bool Delete(long id)
        {
            return repo.orderProductRepo.Delete(id);
        }

        public bool Delete(List<long> ids)
        {
            return repo.orderProductRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public OrderProductModelView Get(long Id)
        {
            return new OrderProductModelView(repo.orderProductRepo.Get(e => e.Id == Id, Includes));
        }

        public OrderProductModelView Get(string textSearch)
        {
            return new OrderProductModelView(repo.orderProductRepo.Get(e => e.Product.Name.Contains("" + textSearch), Includes));
        }

        public List<OrderProductModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.orderProductRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new OrderProductModelView(e)).ToList();
        }
         
        public List<OrderProductModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.orderProductRepo.GetList(e => e.Product.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new OrderProductModelView(e)).ToList();
        }
         
        public IPagedList<OrderProductModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.orderProductRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new OrderProductModelView(e)).ToPagedList(page, pageSize);
        }
         
        public IPagedList<OrderProductModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.orderProductRepo.GetList(e => "" + textSearch == "" || e.Product.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new OrderProductModelView(e)).ToPagedList(page, pageSize);
        }
                 
        public List<OrderProductModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.orderProductRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id) , Includes, Utility.Status.New).Select(e => new OrderProductModelView(e)).ToList();
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.dealerRepo.GetMaXCode();
        }
        #endregion
    }
}