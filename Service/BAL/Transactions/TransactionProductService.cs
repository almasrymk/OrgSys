using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class TransactionProductService : BaseService<TransactionProductModelView>
    {
        string Includes = "Product";
        UnitOfWorkOrg repo;
        public void SetSchema(string Schema)
        {
            if (repo == null)
                repo = new UnitOfWorkOrg(Schema);
        }

        #region Save / Delete
        public TransactionProductModelView Save(TransactionProductModelView ob)
        {
            return new TransactionProductModelView(repo.transactionProductRepo.AddOrUpdate(ob.Model()));
        }
         
        public bool Delete(long id)
        {
            return repo.transactionProductRepo.Delete(id);
        }

        public bool Delete(List<long> ids)
        {
            return repo.transactionProductRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public TransactionProductModelView Get(long Id)
        {
            return new TransactionProductModelView(repo.transactionProductRepo.Get(e => e.Id == Id, Includes));
        }

        public TransactionProductModelView Get(string textSearch)
        {
            return new TransactionProductModelView(repo.transactionProductRepo.Get(e => e.Product.Name.Contains("" + textSearch), Includes));
        }

        public List<TransactionProductModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.transactionProductRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new TransactionProductModelView(e)).ToList();
        }
         
        public List<TransactionProductModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.transactionProductRepo.GetList(e => e.Product.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new TransactionProductModelView(e)).ToList();
        }
         
        public IPagedList<TransactionProductModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.transactionProductRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new TransactionProductModelView(e)).ToPagedList(page, pageSize);
        }
         
        public IPagedList<TransactionProductModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.transactionProductRepo.GetList(e => "" + textSearch == "" || e.Product.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new TransactionProductModelView(e)).ToPagedList(page, pageSize);
        }               
         
        public List<TransactionProductModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.transactionProductRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id) , Includes, Utility.Status.New).Select(e => new TransactionProductModelView(e)).ToList();
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.transactionProductRepo.GetMaXCode();
        }
        #endregion
    }
}