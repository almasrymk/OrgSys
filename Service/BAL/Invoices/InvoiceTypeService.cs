using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class InvoiceTypeService : BaseService<InvoiceTypeModelView>
    {
        string Includes = "";
        UnitOfWork repo;
        public InvoiceTypeService()
        {
            if (repo == null)
                repo = new UnitOfWork();
        }

        #region Save / Delete
        public InvoiceTypeModelView Save(InvoiceTypeModelView ob)
        {
            return new InvoiceTypeModelView(repo.invoiceTypeRepo.AddOrUpdate(ob.Model));
        }
         
        public bool Delete(long id)
        {
            return repo.invoiceTypeRepo.Delete(id);
        }

        public bool Delete(List<long> ids)
        {
            return repo.invoiceTypeRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public InvoiceTypeModelView Get(long Id)
        {
            return new InvoiceTypeModelView(repo.invoiceTypeRepo.Get(e => e.Id == Id , Includes));
        }

        public InvoiceTypeModelView Get(string textSearch)
        {
            return new InvoiceTypeModelView(repo.invoiceTypeRepo.Get(e => e.Name.Contains("" + textSearch) , Includes));
        }

        public List<InvoiceTypeModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.invoiceTypeRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new InvoiceTypeModelView(e)).ToList();
        }
         
        public List<InvoiceTypeModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.invoiceTypeRepo.GetList(e => e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new InvoiceTypeModelView(e)).ToList();
        }
         
        public IPagedList<InvoiceTypeModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.invoiceTypeRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new InvoiceTypeModelView(e)).ToPagedList(page, pageSize);
        }
         
        public IPagedList<InvoiceTypeModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.invoiceTypeRepo.GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new InvoiceTypeModelView(e)).ToPagedList(page, pageSize);
        }
                
        public List<InvoiceTypeModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.invoiceTypeRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new InvoiceTypeModelView(e)).ToList();
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.dealerRepo.GetMaXCode();
        }
        #endregion
    }
}