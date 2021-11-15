using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class OrderTypeService : BaseService<OrderTypeModelView>
    {
        string Includes = "";
        UnitOfWorkOrg repo;
        public void SetSchema(string Schema)
        {
            if (repo == null)
                repo = new UnitOfWorkOrg(Schema);
        }

        #region Save / Delete
        public OrderTypeModelView Save(OrderTypeModelView ob)
        {
            return new OrderTypeModelView(repo.orderTypeRepo.AddOrUpdate(ob.Model()));
        }
         
        public bool Delete(long id)
        {
            return repo.orderTypeRepo.Delete(id);
        }

        public bool Delete(List<long> ids)
        {
            return repo.orderTypeRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public OrderTypeModelView Get(long Id)
        {
            return new OrderTypeModelView(repo.orderTypeRepo.Get(e => e.Id == Id, Includes));
        }

        public OrderTypeModelView Get(string textSearch)
        {
            return new OrderTypeModelView(repo.orderTypeRepo.Get(e => e.Name.Contains("" + textSearch), Includes));
        }

        public List<OrderTypeModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.orderTypeRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new OrderTypeModelView(e)).ToList();
        }
         
        public List<OrderTypeModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.orderTypeRepo.GetList(e => e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new OrderTypeModelView(e)).ToList();
        }
         
        public IPagedList<OrderTypeModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.orderTypeRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new OrderTypeModelView(e)).ToPagedList(page, pageSize);
        }
         
        public IPagedList<OrderTypeModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.orderTypeRepo.GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new OrderTypeModelView(e)).ToPagedList(page, pageSize);
        }
                
        public List<OrderTypeModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.orderTypeRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new OrderTypeModelView(e)).ToList();
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.dealerRepo.GetMaXCode();
        }
        #endregion
    }
}