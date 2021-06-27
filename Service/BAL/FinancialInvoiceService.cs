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
    public class FinancialInvoiceService : BaseService<FinancialInvoiceModelView>
    {
        UnitOfWork repo;
        public FinancialInvoiceService()
        {
            repo = new UnitOfWork();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public FinancialInvoiceModelView Save(FinancialInvoiceModelView ob)
        {
            return new FinancialInvoiceModelView(repo.financialInvoiceRepo.AddOrUpdate(ob.Model));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(long id)
        {
            return repo.financialInvoiceRepo.Delete(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<FinancialInvoiceModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.financialInvoiceRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new FinancialInvoiceModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<FinancialInvoiceModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.financialInvoiceRepo.GetList(null, e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new FinancialInvoiceModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<FinancialInvoiceModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.financialInvoiceRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new FinancialInvoiceModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<FinancialInvoiceModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.financialInvoiceRepo.GetList(e => "" + textSearch == "" , e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new FinancialInvoiceModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FinancialInvoiceModelView Get(long Id)
        {
            return new FinancialInvoiceModelView(repo.financialInvoiceRepo.Get(e => e.Id == Id  , ""));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public FinancialInvoiceModelView Get(string textSearch)
        {
            return new FinancialInvoiceModelView(repo.financialInvoiceRepo.Get(null , ""));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(List<long> ids)
        {
            return repo.financialInvoiceRepo.Delete(ids);
        }

        public List<FinancialInvoiceModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.financialInvoiceRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id) , "", Utility.Status.New).Select(e => new FinancialInvoiceModelView(e)).ToList();
        }
    }
}