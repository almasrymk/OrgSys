using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class PaymentTypeService : BaseService<PaymentTypeModelView>
    {
        string Includes = "";
        UnitOfWork repo;
        public PaymentTypeService()
        {
            repo = new UnitOfWork();
        }

        #region Save / Delete
        public PaymentTypeModelView Save(PaymentTypeModelView ob)
        {
            return new PaymentTypeModelView(repo.paymentTypeRepo.AddOrUpdate(ob.Model()));
        }
      
        public bool Delete(long id)
        {
            return repo.paymentTypeRepo.Delete(id);
        }

        public bool Delete(List<long> ids)
        {
            return repo.paymentTypeRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public PaymentTypeModelView Get(long Id)
        {
            return new PaymentTypeModelView(repo.paymentTypeRepo.Get(e => e.Id == Id , Includes));
        }

        public PaymentTypeModelView Get(string textSearch)
        {
            return new PaymentTypeModelView(repo.paymentTypeRepo.Get(e => e.Name.Contains("" + textSearch) , Includes));
        }

        public List<PaymentTypeModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.paymentTypeRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new PaymentTypeModelView(e)).ToList();
        }
       
        public List<PaymentTypeModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.paymentTypeRepo.GetList(e => e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new PaymentTypeModelView(e)).ToList();
        }
        
        public IPagedList<PaymentTypeModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.paymentTypeRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new PaymentTypeModelView(e)).ToPagedList(page, pageSize);
        }
     
        public IPagedList<PaymentTypeModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.paymentTypeRepo.GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new PaymentTypeModelView(e)).ToPagedList(page, pageSize);
        }             
 
        public List<PaymentTypeModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.paymentTypeRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new PaymentTypeModelView(e)).ToList();
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.paymentTypeRepo.GetMaXCode();
        }
        #endregion
    }
}