using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class FinancialInvoiceService : BaseService<FinancialInvoiceModelView>
    {
        string Includes = "";
        UnitOfWork repo;
        public FinancialInvoiceService()
        {
            repo = new UnitOfWork();
        }

        #region Save / Delete
        public FinancialInvoiceModelView Save(FinancialInvoiceModelView ob)
        {
            return new FinancialInvoiceModelView(repo.financialInvoiceRepo.AddOrUpdate(ob.Model));
        }
         
        public bool Delete(long id)
        {
            return repo.financialInvoiceRepo.Delete(id);
        }

        public bool Delete(List<long> ids)
        {
            return repo.financialInvoiceRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public FinancialInvoiceModelView Get(long Id)
        {
            return new FinancialInvoiceModelView(repo.financialInvoiceRepo.Get(e => e.Id == Id, Includes));
        }

        public FinancialInvoiceModelView Get(string textSearch)
        {
            return new FinancialInvoiceModelView(repo.financialInvoiceRepo.Get(null, Includes));
        }

        public List<FinancialInvoiceModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.financialInvoiceRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new FinancialInvoiceModelView(e)).ToList();
        }
         
        public List<FinancialInvoiceModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.financialInvoiceRepo.GetList(null, e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new FinancialInvoiceModelView(e)).ToList();
        }
         
        public IPagedList<FinancialInvoiceModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.financialInvoiceRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new FinancialInvoiceModelView(e)).ToPagedList(page, pageSize);
        }
         
        public IPagedList<FinancialInvoiceModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.financialInvoiceRepo.GetList(e => "" + textSearch == Includes, e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new FinancialInvoiceModelView(e)).ToPagedList(page, pageSize);
        }
                 
        public List<FinancialInvoiceModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.financialInvoiceRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id) , Includes, Utility.Status.New).Select(e => new FinancialInvoiceModelView(e)).ToList();
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.financialInvoiceRepo.GetMaXCode();
        }
        #endregion
    }
}