using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class InvoiceProductService : IBaseService<InvoiceProductModelView>
    {
        string Includes = "Product";
        UnitOfWorkOrg repo;
        private string _Schema;
        public InvoiceProductService(string Schema)
        {
            this._Schema = Schema;
            repo = new UnitOfWorkOrg(Schema);
        }

        #region Save / Delete
        public InvoiceProductModelView Save(InvoiceProductModelView ob)
        {
            return new InvoiceProductModelView(repo.invoiceProductRepo.AddOrUpdate(ob.Model()));
        }
         
        public bool Delete(long id)
        {
            return repo.invoiceProductRepo.Delete(id);
        }

        public bool Delete(List<long> ids)
        {
            return repo.invoiceProductRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public InvoiceProductModelView Get(long Id)
        {
            return new InvoiceProductModelView(repo.invoiceProductRepo.Get(e => e.Id == Id, Includes));
        }

        public InvoiceProductModelView Get(string textSearch)
        {
            return new InvoiceProductModelView(repo.invoiceProductRepo.Get(e => e.Product.Name.Contains("" + textSearch), Includes));
        }

        public List<InvoiceProductModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.invoiceProductRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new InvoiceProductModelView(e)).ToList();
        }
         
        public List<InvoiceProductModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.invoiceProductRepo.GetList(e => e.Product.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new InvoiceProductModelView(e)).ToList();
        }
         
        public IPagedList<InvoiceProductModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.invoiceProductRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new InvoiceProductModelView(e)).ToPagedList(page, pageSize);
        }
         
        public IPagedList<InvoiceProductModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.invoiceProductRepo.GetList(e => "" + textSearch == "" || e.Product.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new InvoiceProductModelView(e)).ToPagedList(page, pageSize);
        }
                
        public List<InvoiceProductModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.invoiceProductRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id) , Includes, Utility.Status.New).Select(e => new InvoiceProductModelView(e)).ToList();
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.invoiceProductRepo.GetMaXCode();
        }
        #endregion
    }
}