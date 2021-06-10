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
    public class InvoiceService : BaseService<InvoiceModelView>
    {
        UnitOfWork repo;
        public InvoiceService()
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
            // Save
            var Nwob = repo.invoiceRepo.AddOrUpdate(ob.Model);

            if (ob.Id > 0)
            {
                var ids = ob.InvoiceProducts.Select(e => e.Id).ToList();
                if (ids == null) ids = new List<long>();

                // Delete row from database
                var deleted = repo.invoiceProductRepo.GetList(e => e.InvoiceId == ob.Id && !ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
                if (deleted != null && deleted.Count > 0)
                    repo.invoiceProductRepo.ShiftDelete(deleted.Select(e => e.Id).ToList());

                foreach (var productUnit in ob.InvoiceProducts)
                {
                    var model = productUnit.Model;
                    model.InvoiceId = Nwob.Id;
                    repo.invoiceProductRepo.AddOrUpdate(model);
                }
                Nwob.InvoiceProducts = repo.invoiceProductRepo.GetList(e => e.InvoiceId == Nwob.Id, e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
            }
            return new InvoiceModelView(Nwob);
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
            return repo.invoiceRepo.GetList( e=>e.TypeId == TypeId, e => e.OrderByDescending(e => e.Id), "Dealer", Utility.Status.New).Select(e => new InvoiceModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<InvoiceModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.invoiceRepo.GetList(e => e.Dealer.Name.Contains("" + textSearch) && e.TypeId == TypeId, e => e.OrderByDescending(e => e.Id), "Dealer", Utility.Status.New).Select(e => new InvoiceModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<InvoiceModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.invoiceRepo.GetList(e=> e.TypeId == TypeId , e => e.OrderByDescending(e => e.Id), "Dealer", Utility.Status.New).Select(e => new InvoiceModelView(e)).ToPagedList(page, pageSize);
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
            return repo.invoiceRepo.GetList(e => e.TypeId == TypeId && ("" + textSearch == "" || e.Dealer.Name.Contains("" + textSearch)), e => e.OrderByDescending(e => e.Id), "Dealer", Utility.Status.New).Select(e => new InvoiceModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InvoiceModelView Get(long Id)
        {
            return new InvoiceModelView(repo.invoiceRepo.Get(e => e.Id == Id  , "Dealer,InvoiceProducts,InvoiceProducts.Product,InvoiceProducts.Product.ProductUnits,,InvoiceProducts.Product.ProductUnits.Unit"));
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

        public List<InvoiceModelView> GetAll(List<long> ids,long TypeId = 0)
        {
            return repo.invoiceRepo.GetList(e => e.TypeId == TypeId && ids.Contains(e.Id), e => e.OrderBy(e => e.Id) , "Dealer", Utility.Status.New).Select(e => new InvoiceModelView(e)).ToList();
        }
       // , long TypeId
        public long GetMaxCode(long type )
        {
            return repo.invoiceRepo.GetMaXCode(type);
        }
    }
}