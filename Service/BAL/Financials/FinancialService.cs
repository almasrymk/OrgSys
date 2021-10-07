using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class FinancialService : BaseService<FinancialModelView>
    {
        string Includes = "Dealer,Outlay,Safe,FinancialInvoices,FinancialInvoices.Invoice";
        UnitOfWork repo;
        public FinancialService()
        {
            repo = new UnitOfWork();
        }

        #region Save / Delete
        public FinancialModelView Save(FinancialModelView ob)
        {
            // Save
            var Nwob = repo.financialRepo.AddOrUpdate(ob.Model);
            if (ob.FinancialInvoices == null)
                ob.FinancialInvoices = new List<FinancialInvoiceModelView>();

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

            if (Nwob.FinancialInvoices != null && Nwob.FinancialInvoices.Count > 0)
                new InvoiceService().UpdateCredit(Nwob.FinancialInvoices.Select(e => e.InvoiceId).ToList());
            return new FinancialModelView(Nwob);
        }

        public bool Delete(long id)
        {
            bool res = false;
            var ob = repo.financialRepo.Get(e => e.Id == id);
            if (ob != null)
            {
                res = repo.financialRepo.Delete(id);
                if (res)
                {
                    var InvIds = ob.FinancialInvoices.Select(e => e.InvoiceId).ToList();
                    //new InvoiceService().UpdateCredit(InvIds);
                }
            }

            return res;
        }

        public bool Delete(List<long> ids)
        {
            return repo.financialRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public FinancialModelView Get(long Id)
        {
            return new FinancialModelView(repo.financialRepo.Get(e => e.Id == Id, Includes));
        }

        public FinancialModelView Get(string textSearch)
        {
            return new FinancialModelView(repo.financialRepo.Get(e => e.Dealer.Name.Contains("" + textSearch), Includes));
        }

        public FinancialModelView GetByParent(long Id)
        {
            return new FinancialModelView(repo.financialRepo.Get(e => e.ParentId == Id && e.Status != Utility.Status.Deleted, Includes));
        }

        public List<FinancialModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.financialRepo.GetList(e => e.TypeId == TypeId, e => e.OrderByDescending(e => e.Id), Includes, Utility.Status.New).Select(e => new FinancialModelView(e)).ToList();
        }

        public List<FinancialModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0)
        {
            return repo.financialRepo.GetList(e => e.Dealer.Name.Contains("" + textSearch) && e.TypeId == TypeId, e => e.OrderByDescending(e => e.Id), Includes, Utility.Status.New).Select(e => new FinancialModelView(e)).ToList();
        }

        public IPagedList<FinancialModelView> GetAll(long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.financialRepo.GetList(e => e.TypeId == TypeId, e => e.OrderByDescending(e => e.Id), Includes, Utility.Status.New).Select(e => new FinancialModelView(e)).ToPagedList(page, pageSize);
        }

        public IPagedList<FinancialModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.financialRepo.GetList(e => e.TypeId == TypeId && ("" + textSearch == "" || e.Dealer.Name.Contains("" + textSearch)), e => e.OrderByDescending(e => e.Id), Includes, Utility.Status.New).Select(e => new FinancialModelView(e)).ToPagedList(page, pageSize);
        }

        public List<FinancialModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.financialRepo.GetList(e => e.TypeId == TypeId && ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new FinancialModelView(e)).ToList();
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.financialRepo.GetMaXCode(e => e.TypeId == type);
        }
        #endregion

        #region Integration
        public bool CreateFinancialByInvoice(InvoiceModelView inv)
        {
            if (inv != null && inv.Id > 0 && inv.Paid - inv.Remaining + inv.Credit != inv.Net)
            {
                //new FinancialInvoiceService().GetAll()
                var financial = new FinancialModelView(inv.Model);
                financial.SafeId = long.Parse("0" + new PreferenceService().GetByKey("DefaultSafe", "Financial", (inv.TypeId == 1 || inv.TypeId == 4 ? 1 : 2), 0)?.Value);
                financial.PaymentTypeId = long.Parse("0" + new PreferenceService().GetByKey("DefaultPaymentType", "Financial", (inv.TypeId == 1 || inv.TypeId == 4 ? 1 : 2), 0)?.Value);
                inv.CodeNumber = GetMaxCode(inv.TypeId == 1 || inv.TypeId == 4 ? 1 : 2);
                inv.Code = "" + inv.CodeNumber;
                Save(financial);
                return true;
            }
            return false;
        }
        #endregion
    }
}