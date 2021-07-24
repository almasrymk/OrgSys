using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrgSys.Controllers;
using Service;
using System;
using System.Collections.Generic;

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
            ViewBag.SafeId = new SelectList(new SafeService().GetAll(model.ParentId, 0, 1, 20), "Id", "Name", model.SafeId);
            var type = new TransactionTypeService().Get(model.TypeId);
            ViewBag.TransactionsType = type.Name;
            ViewBag.TransactionsType = type.Icon;
        }

        public override FinancialModelView InitializeData(FinancialModelView ob)
        {
            var setting = new PreferenceService();
            var SafeId = long.Parse("0" + setting.GetByKey("DefaultSafe", "Financial", ob.TypeId, 0)?.Value);
            var PaymentTypeId = long.Parse("0" + setting.GetByKey("DefaultPaymentType", "Financial", ob.TypeId, 0)?.Value);
            var CurrencyId = long.Parse("0" + setting.GetByKey("DefaultCurrency", "Financial", ob.TypeId, 0)?.Value);
            var OutlayId = long.Parse("0" + setting.GetByKey("DefaultOutlay", "Financial", ob.TypeId, 0)?.Value);

            long DealerId = 0;
            if(ob.TypeId == 1)
                DealerId = long.Parse("0" + setting.GetByKey("DefaultSupplier", "Financial", ob.TypeId, 0)?.Value);
            else if (ob.TypeId == 2)
                DealerId = long.Parse("0" + setting.GetByKey("DefaultCustomer", "Financial", ob.TypeId, 0)?.Value);
            
            ViewBag.AutoSave = int.Parse("0" + setting.GetByKey("AutoSave", "Financial", ob.TypeId, 0)?.Value);
            var TypeCode = int.Parse("0" + setting.GetByKey("TypeSerial", "Financial", ob.TypeId, 0)?.Value);
            ViewBag.TypeSerial = TypeCode;

            if (ob == null)
                ob = new FinancialModelView();

            if (ob.Id == 0)
            {
                ob.CodeNumber = new FinancialService().GetMaxCode(ob.TypeId);
                ob.Code = "" + ob.CodeNumber;
                ob.SafeId = SafeId;
                ob.DealerId = DealerId;
                ob.CurrencyId = long.Parse("0" + CurrencyId);
                ob.Rate = new CurrencyService().Get(long.Parse("0" + CurrencyId))?.Rate??0;
                ob.PaymentTypeId = PaymentTypeId;
                ob.OutlayId = OutlayId;
                ob.Date = DateTime.Now;
                ob.FinancialInvoices = new List<FinancialInvoiceModelView>();
            }

            ob.SafeName = new SafeService().Get(ob.SafeId).Name;
            ob.DealerName = new DealerService().Get(ob.DealerId??0).Name;
            return ob;
        }      
    }
}