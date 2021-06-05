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
    public class InvoiceProductService : BaseService<InvoiceProductModelView>
    {
        UnitOfWork repo;
        public InvoiceProductService()
        {
            repo = new UnitOfWork();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public InvoiceProductModelView Save(InvoiceProductModelView ob)
        {
            return new InvoiceProductModelView(repo.invoiceProductRepo.AddOrUpdate(ob.Model));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(long id)
        {
            return repo.invoiceProductRepo.Delete(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<InvoiceProductModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.invoiceProductRepo.GetList(e => e.OrderBy(e => e.Id), "Product", Utility.Status.New).Select(e => new InvoiceProductModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<InvoiceProductModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.invoiceProductRepo.GetList(e => e.Product.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "Product", Utility.Status.New).Select(e => new InvoiceProductModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<InvoiceProductModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.invoiceProductRepo.GetList(e => e.OrderBy(e => e.Id), "Product", Utility.Status.New).Select(e => new InvoiceProductModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<InvoiceProductModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.invoiceProductRepo.GetList(e => "" + textSearch == "" || e.Product.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "Product", Utility.Status.New).Select(e => new InvoiceProductModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InvoiceProductModelView Get(long Id)
        {
            return new InvoiceProductModelView(repo.invoiceProductRepo.Get(e => e.Id == Id  , "Product"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public InvoiceProductModelView Get(string textSearch)
        {
            return new InvoiceProductModelView(repo.invoiceProductRepo.Get(e => e.Product.Name.Contains("" + textSearch) , "Product"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(List<long> ids)
        {
            return repo.invoiceProductRepo.Delete(ids);
        }

        public List<InvoiceProductModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.invoiceProductRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id) , "Product", Utility.Status.New).Select(e => new InvoiceProductModelView(e)).ToList();
        }
    }
}