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
    public class ReturnInvoiceService : BaseService<InvoiceModelView>
    {
        UnitOfWork repo;
        public ReturnInvoiceService()
        {
            repo = new UnitOfWork();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public InvoiceModelView Save(InvoiceModelView ob)
        {
            return new InvoiceModelView(repo.invoiceRepo.AddOrUpdate(ob.Model));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(long id)
        {
            return repo.invoiceRepo.Delete(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<InvoiceModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.invoiceRepo.GetList(e => e.OrderBy(e => e.Id), "Dealer", Utility.Status.New).Select(e => new InvoiceModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<InvoiceModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.invoiceRepo.GetList(e => e.Dealer.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "Dealer", Utility.Status.New).Select(e => new InvoiceModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<InvoiceModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.invoiceRepo.GetList(e => e.OrderBy(e => e.Id), "Dealer", Utility.Status.New).Select(e => new InvoiceModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<InvoiceModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.invoiceRepo.GetList(e => "" + textSearch == "" || e.Dealer.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "Dealer", Utility.Status.New).Select(e => new InvoiceModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InvoiceModelView Get(long Id)
        {
            return new InvoiceModelView(repo.invoiceRepo.Get(e => e.Id == Id  , "Dealer"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public InvoiceModelView Get(string textSearch)
        {
            return new InvoiceModelView(repo.invoiceRepo.Get(e => e.Dealer.Name.Contains("" + textSearch) , "Dealer"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(List<long> ids)
        {
            return repo.invoiceRepo.Delete(ids);
        }

        public List<InvoiceModelView> GetAll(List<long> ids)
        {
            return repo.invoiceRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id) , "Dealer", Utility.Status.New).Select(e => new InvoiceModelView(e)).ToList();
        }
    }
}