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
    public class InvoiceTypeService : BaseService<InvoiceTypeModelView>
    {
        UnitOfWork repo;
        public InvoiceTypeService()
        {
            if (repo == null)
                repo = new UnitOfWork();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public InvoiceTypeModelView Save(InvoiceTypeModelView ob)
        {
            return new InvoiceTypeModelView(repo.invoiceTypeRepo.AddOrUpdate(ob.Model));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(long id)
        {
            return repo.invoiceTypeRepo.Delete(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<InvoiceTypeModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.invoiceTypeRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new InvoiceTypeModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<InvoiceTypeModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.invoiceTypeRepo.GetList(e => e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new InvoiceTypeModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<InvoiceTypeModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.invoiceTypeRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new InvoiceTypeModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<InvoiceTypeModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.invoiceTypeRepo.GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new InvoiceTypeModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InvoiceTypeModelView Get(long Id)
        {
            return new InvoiceTypeModelView(repo.invoiceTypeRepo.Get(e => e.Id == Id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public InvoiceTypeModelView Get(string textSearch)
        {
            return new InvoiceTypeModelView(repo.invoiceTypeRepo.Get(e => e.Name.Contains("" + textSearch)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(List<long> ids)
        {
            return repo.invoiceTypeRepo.Delete(ids);
        }

        public List<InvoiceTypeModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.invoiceTypeRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new InvoiceTypeModelView(e)).ToList();
        }
    }
}