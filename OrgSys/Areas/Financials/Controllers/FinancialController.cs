using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrgSys.Controllers;
using Service.BAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace OrgSys.Areas.Financial.Controllers
{
    [Area("Financials")]
    public class FinancialController : BaseController<FinancialModelView>
    {
        public override void LoadViewBagIndex(long ParentId = 0, long TypeId = 0)
        {
            var type = new FinancialTypeService().Get(TypeId);
            ViewBag.FinancialsType = type.Name;
            ViewBag.FinancialsIcon = type.Icon;

            base.LoadViewBagIndex();
        }

        public override void LoadViewBag(FinancialModelView model)
        {
            ViewBag.OutlayId = new SelectList(new OutlayService().GetAll(model.ParentId, 0, 1, 20), "Id", "Name", model.OutlayId);
            ViewBag.CurrencyId = new SelectList(new CurrencyService().GetAll(model.ParentId, 0, 1, 20), "Id", "Name", model.CurrencyId);
            ViewBag.PaymentTypeId = new SelectList(new PaymentTypeService().GetAll(model.ParentId, 0, 1, 20), "Id", "Name", model.PaymentTypeId);
            ViewBag.DealerId = new SelectList(new DealerService().GetAll(model.ParentId, model.TypeId == 1 || model.TypeId == 3 ? (int)DealerType.Client : (int)DealerType.Supplier, 1, 20), "Id", "Name", model.DealerId);
            ViewBag.SafeId = new SelectList(new SafeService().GetAll(model.ParentId, 0, 1, 20), "Id", "Name", model.SafeId);
            var type = new TransactionTypeService().Get(model.TypeId);
            ViewBag.TransactionsType = type.Name;
            ViewBag.TransactionsType = type.Icon;
        }

        public override FinancialModelView InitializeData(FinancialModelView ob)
        {
            var setting = new PreferenceService();
            var SafeId = long.Parse("0" + setting.GetByKey("DefaultSafe", "Financial", ob.TypeId, 0)?.Value);

            long DealerId = 0;
            if(ob.TypeId == 1)
                DealerId = long.Parse("0" + setting.GetByKey("DefaultSupplier", "Financial", ob.TypeId, 0)?.Value);
            else if (ob.TypeId == 2)
                DealerId = long.Parse("0" + setting.GetByKey("DefaultCustomer", "Financial", ob.TypeId, 0)?.Value);

            ViewBag.OrderTabe = int.Parse("0" + setting.GetByKey("OrderTabe", "Financial", ob.TypeId, 0)?.Value);
            ViewBag.AutoSave = int.Parse("0" + setting.GetByKey("AutoSave", "Financial", ob.TypeId, 0)?.Value);
            var TypeCode = int.Parse("0" + setting.GetByKey("TypeSerial", "Financial", ob.TypeId, 0)?.Value);
            ViewBag.TypeSerial = TypeCode;

            if (ob == null)
                ob = new FinancialModelView();

            if (ob.Id == 0)
            {
                ob.CodeNumber = new FinancialService().GetMaxCode(ob.TypeId);
                ob.Code = "" + new FinancialService().GetMaxCode(ob.TypeId);
                ob.SafeId = SafeId;
                ob.DealerId = DealerId;
                ob.Date = DateTime.Now;
                ob.FinancialInvoices = new List<FinancialInvoiceModelView>();
            }
            
            return ob;
        }       
    }
}
