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
    public class FinancialService : BaseService<FinancialModelView>
    {
        UnitOfWork repo;
        public FinancialService()
        {
            repo = new UnitOfWork();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public FinancialModelView Save(FinancialModelView ob)
        {         
            // Save
            var Nwob = repo.financialRepo.AddOrUpdate(ob.Model);

            if (ob.Id > 0)
            {
                var ids = ob.FinancialInvoices.Select(e => e.Id).ToList();
                if (ids == null) ids = new List<long>();

                // Delete row from database
                var deleted = repo.financialInvoiceRepo.GetList(e => e.FinancialId == ob.Id && !ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "Invoice", Utility.Status.All).ToList();
                if (deleted != null && deleted.Count > 0)
                    repo.financialInvoiceRepo.ShiftDelete(deleted.Select(e => e.Id).ToList());

                foreach (var productUnit in ob.FinancialInvoices)
                {
                    var model = productUnit.Model;
                    model.FinancialId = Nwob.Id;
                    repo.financialInvoiceRepo.AddOrUpdate(model);
                }
                Nwob.FinancialInvoices = repo.financialInvoiceRepo.GetList(e => e.FinancialId == Nwob.Id, e => e.OrderBy(e => e.Id), "Invoice", Utility.Status.All).ToList();
            }
           
            return new FinancialModelView(Nwob);
        }            

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(long id)
        {
            return repo.financialRepo.Delete(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<FinancialModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.financialRepo.GetList( e=>e.TypeId == TypeId, e => e.OrderByDescending(e => e.Id), "Dealer,Safe", Utility.Status.New).Select(e => new FinancialModelView(e)).ToList();            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<FinancialModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.financialRepo.GetList(e => e.Dealer.Name.Contains("" + textSearch) && e.TypeId == TypeId, e => e.OrderByDescending(e => e.Id), "Dealer,Safe", Utility.Status.New).Select(e => new FinancialModelView(e)).ToList();            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<FinancialModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return  repo.financialRepo.GetList(e=> e.TypeId == TypeId , e => e.OrderByDescending(e => e.Id), "Dealer,Safe", Utility.Status.New).Select(e => new FinancialModelView(e)).ToPagedList(page, pageSize);           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<FinancialModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.financialRepo.GetList(e => e.TypeId == TypeId && ("" + textSearch == "" || e.Dealer.Name.Contains("" + textSearch)), e => e.OrderByDescending(e => e.Id), "Dealer,Safe", Utility.Status.New).Select(e => new FinancialModelView(e)).ToPagedList(page, pageSize);            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FinancialModelView Get(long Id)
        {
            return new FinancialModelView(repo.financialRepo.Get(e => e.Id == Id  , "Dealer,Safe"));            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public FinancialModelView Get(string textSearch)
        {
            return new FinancialModelView(repo.financialRepo.Get(e => e.Dealer.Name.Contains("" + textSearch) , "Dealer,Safe"));            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(List<long> ids)
        {
            return repo.financialRepo.Delete(ids);
        }

        public List<FinancialModelView> GetAll(List<long> ids,long TypeId = 0)
        {
            return repo.financialRepo.GetList(e => e.TypeId == TypeId && ids.Contains(e.Id), e => e.OrderBy(e => e.Id) , "Dealer,Safe", Utility.Status.New).Select(e => new FinancialModelView(e)).ToList();
        }
       
        public long GetMaxCode(long type )
        {
            return repo.financialRepo.GetMaXCode(type);
        }
    }
}