using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class TransactionTypeService : BaseService<TransactionTypeModelView>
    {
        string Includes = "";
        UnitOfWorkOrg repo;
        public void SetSchema(string Schema)
        {
            if (repo == null)
                repo = new UnitOfWorkOrg(Schema);
        }

        #region Save / Delete
        public TransactionTypeModelView Save(TransactionTypeModelView ob)
        {
            return new TransactionTypeModelView(repo.transactionTypeRepo.AddOrUpdate(ob.Model()));
        }
         
        public bool Delete(long id)
        {
            return repo.transactionTypeRepo.Delete(id);
        }

        public bool Delete(List<long> ids)
        {
            return repo.transactionTypeRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public TransactionTypeModelView Get(long Id)
        {
            return new TransactionTypeModelView(repo.transactionTypeRepo.Get(e => e.Id == Id , Includes));
        }

        public TransactionTypeModelView Get(string textSearch)
        {
            return new TransactionTypeModelView(repo.transactionTypeRepo.Get(e => e.Name.Contains("" + textSearch) , Includes));
        }

        public List<TransactionTypeModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.transactionTypeRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new TransactionTypeModelView(e)).ToList();
        }
         
        public List<TransactionTypeModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.transactionTypeRepo.GetList(e => e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new TransactionTypeModelView(e)).ToList();
        }
         
        public IPagedList<TransactionTypeModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.transactionTypeRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new TransactionTypeModelView(e)).ToPagedList(page, pageSize);
        }
         
        public IPagedList<TransactionTypeModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.transactionTypeRepo.GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new TransactionTypeModelView(e)).ToPagedList(page, pageSize);
        }
                
        public List<TransactionTypeModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.transactionTypeRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new TransactionTypeModelView(e)).ToList();
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.transactionTypeRepo.GetMaXCode();
        }
        #endregion
    }
}