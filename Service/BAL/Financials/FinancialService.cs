using Entity;
using System.Linq;
using Entity.Model;
using Entity.ModelView;
using System.Collections.Generic;

namespace Service
{
    public class FinancialService : BaseOrgService<FinancialModelView, Financial>
    {
        public FinancialService(string Schema) : base(Schema , "Dealer,PaymentType,Outlay,Currency,Safe,FinancialInvoices") { }

        public override FinancialModelView Save(FinancialModelView ob)
        {
            // Save
            var Nwob = repo .AddOrUpdate(ob.Map<Financial>());
            if (ob.FinancialInvoiceList == null)
                ob.FinancialInvoiceList = new List<FinancialInvoiceModelView>();

            if (ob.Id > 0)
            {
                var ids = ob.FinancialInvoiceList.Select(e => e.Id).ToList();
                if (ids == null) ids = new List<long>();

                // Delete row from database
                var deleted = repoAll.financialInvoiceRepo.GetList(e => e.FinancialId == ob.Id && !ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.All).ToList();
                if (deleted != null && deleted.Count > 0)
                    repoAll.financialInvoiceRepo.ShiftDelete(deleted.Select(e => e.Id).ToList());

                foreach (var productUnit in ob.FinancialInvoices)
                {
                    var model = productUnit.Map<FinancialInvoice>();
                    model.FinancialId = Nwob.Id;
                    repoAll.financialInvoiceRepo.AddOrUpdate(model);
                }
                Nwob.FinancialInvoices = repoAll.financialInvoiceRepo.GetList(e => e.FinancialId == Nwob.Id, e => e.OrderBy(e => e.Id), "Invoice", Utility.Status.All).ToList();
            }

            if (Nwob.FinancialInvoices != null && Nwob.FinancialInvoices.Count > 0)
                new IntegrationServics(_Schema).UpdateCredit(Nwob.FinancialInvoices.Select(e => e.InvoiceId ?? 0).ToList());
            return Nwob.Map<FinancialModelView>();
        }

        public override bool Delete(long id)
        {
            bool res = false;
            var ob = repo .Get(e => e.Id == id, Includes);
            if (ob != null)
            {
                repo .Delete(id);
                if (ob.FinancialInvoices != null && ob.FinancialInvoices.Count > 0)
                    new IntegrationServics(_Schema).UpdateCredit(ob.FinancialInvoices.Select(e => e.InvoiceId ?? 0).ToList());
            }

            return res;
        }

        public override bool Delete(List<long> ids)
        {
            bool res = false;
            var obList = repo .GetList(e => ids.Contains(e.Id), Includes);
            if (obList != null && obList.Count() > 0)
            {
                res = repo .Delete(ids);
                foreach (var ob in obList)
                    if (ob.FinancialInvoices != null && ob.FinancialInvoices.Count > 0)
                        new IntegrationServics(_Schema).UpdateCredit(ob.FinancialInvoices.Select(e => e.InvoiceId ?? 0).ToList());
            }
            return res;
        }

        public void Cancel(long Id)
        {
            var ob = repo .Get(e => e.Id == Id, Includes);
            ob.Status = Utility.Status.Cancel;
            ob = repo .AddOrUpdate(ob);
            if (ob.FinancialInvoices != null && ob.FinancialInvoices.Count > 0)
                new IntegrationServics(_Schema).UpdateCredit(ob.FinancialInvoices.Select(e => e.InvoiceId ?? 0).ToList());
        }

        public void Redo(long Id)
        {
            var ob = repo .Get(e => e.Id == Id, Includes);
            ob.Status = Utility.Status.All;
            ob = repo .AddOrUpdate(ob);
            if (ob.FinancialInvoices != null && ob.FinancialInvoices.Count > 0)
                new IntegrationServics(_Schema).UpdateCredit(ob.FinancialInvoices.Select(e => e.InvoiceId ?? 0).ToList());
        }

        public FinancialModelView GetByParent(long Id)
        {
            return repo .Get(e => e.ParentId == Id && e.Status != Utility.Status.Deleted, Includes).Map<FinancialModelView>();
        }
    }
}