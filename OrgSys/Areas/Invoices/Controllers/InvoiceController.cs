using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrgSys.Controllers;
using Service.BAL;
using System;
using System.Collections.Generic;
using System.Linq;
using Utility;

namespace OrgSys.Areas.Invoices.Controllers
{
    [Area("Invoices")]
    public class InvoiceController : BaseController<InvoiceModelView>
    {
        public override void LoadViewBagIndex(long ParentId = 0, long TypeId = 0)
        {
            var type = new InvoiceTypeService().Get(TypeId);
            ViewBag.InvoicesType = type.Name;
            ViewBag.InvoicesGroup = type.Group;
            ViewBag.InvoicesIcon = type.Icon;

            base.LoadViewBagIndex();
        }

        public override void LoadViewBag(InvoiceModelView model)
        {
            ViewBag.CurrencyId = new SelectList(new CurrencyService().GetAll(model.ParentId, 0, 1, 20), "Id", "Name", model.CurrencyId);
            ViewBag.PaymentTypeId = new SelectList(new PaymentTypeService().GetAll(model.ParentId, model.PaymentTypeId, 1, 20), "Id", "Name", model.PaymentTypeId);
            
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem { Value = "1", Text = "Amount" });
            selectListItems.Add(new SelectListItem { Value = "2", Text = "Percentage" });

            ViewBag.DiscountType = new SelectList(selectListItems, "Value", "Text");
            ViewBag.ServiceType = new SelectList(selectListItems, "Value", "Text");
            ViewBag.TaxType = new SelectList(selectListItems, "Value", "Text");

            var type = new InvoiceTypeService().Get(model.TypeId);
            ViewBag.InvoicesType = type.Name;
            ViewBag.InvoicesGroup = type.Group;
            ViewBag.InvoicesIcon = type.Icon;
        }

        public override InvoiceModelView InitializeData(InvoiceModelView ob)
        {
            var setting = new PreferenceService();
            var StoreId = long.Parse("0" + setting.GetByKey("DefaultStore", "Invoice", ob.TypeId, 0)?.Value);

            long DealerId = 0;
            if (ob.TypeId == 1 || ob.TypeId == 3)
                DealerId = long.Parse("0" + setting.GetByKey("DefaultCustomer", "Invoice", ob.TypeId, 0)?.Value);
            else if (ob.TypeId == 2 || ob.TypeId == 4)
                DealerId = long.Parse("0" + setting.GetByKey("DefaultSupplier", "Invoice", ob.TypeId, 0)?.Value);

            var PaymentTypeId = long.Parse("0" + setting.GetByKey("DefaultPaymentType", "Invoice", ob.TypeId, 0)?.Value);
            var DefaultDiscountType = int.Parse("0" + setting.GetByKey("DefaultDiscountType", "Invoice", ob.TypeId, 0)?.Value);
            var DefaultServiceType = int.Parse("0" + setting.GetByKey("DefaultServiceType", "Invoice", ob.TypeId, 0)?.Value);
            var DefaultTaxType = int.Parse("0" + setting.GetByKey("DefaultTaxType", "Invoice", ob.TypeId, 0)?.Value);
            var DiscountValue = decimal.Parse("0" + setting.GetByKey("DiscountValue", "Invoice", ob.TypeId, 0)?.Value);
            var ServiceValue = decimal.Parse("0" + setting.GetByKey("ServiceValue", "Invoice", ob.TypeId, 0)?.Value);
            var TaxValue = decimal.Parse("0" + setting.GetByKey("TaxValue", "Invoice", ob.TypeId, 0)?.Value);
            ViewBag.NumberLine = int.Parse("0" + setting.GetByKey("NumberLine", "Invoice", ob.TypeId, 0)?.Value);
            ViewBag.OrderTabe = int.Parse("0" + setting.GetByKey("OrderTabe", "Invoice", ob.TypeId, 0)?.Value);
            ViewBag.AutoSave = int.Parse("0" + setting.GetByKey("AutoSave", "Invoice", ob.TypeId, 0)?.Value);
            var TypeCode = int.Parse("0" + setting.GetByKey("TypeSerial", "Invoice", ob.TypeId, 0)?.Value);
            ViewBag.TypeSerial = TypeCode;
            ViewBag.AllowRepeated = int.Parse("0" + setting.GetByKey("AllowRepeated", "Invoice", ob.TypeId, 0)?.Value);

            if (ob == null)
                ob = new InvoiceModelView();

            if (ob.Id == 0)
            {
                ob.CodeNumber = new InvoiceService().GetMaxCode(ob.TypeId);
                ob.Code = "" + new InvoiceService().GetMaxCode(ob.TypeId);
                ob.StoreId = StoreId;               
                ob.DealerId = DealerId;                
                ob.PaymentTypeId = PaymentTypeId;
                ob.Date = DateTime.Now;
                ob.DiscountType = DefaultDiscountType;
                ob.ServiceType = DefaultServiceType;
                ob.TaxType = DefaultTaxType;
                ob.Discount = DiscountValue;
                ob.Service = ServiceValue;
                ob.Tax = TaxValue;
                ob.InvoiceProducts = new List<InvoiceProductModelView>();
              }

            ob.StoreName = new StoreService().Get(ob.StoreId).Name;
            ob.DealerName = new DealerService().Get(ob.DealerId).Name;
            ob.ParentCode = new InvoiceService().Get(ob.ParentId).Code;
            return ob;
        }

        [HttpGet]
        public virtual ActionResult CreateTransaction(long id, string search, long ParentId = 0, long TypeId = 0, int page = 1)
        {
            var ob = new InvoiceService().CreateTransaction(id);
            return Redirect("/Invoices/Invoice/Index?ParentId=" + ParentId + "&TypeId=" + TypeId + "&page=" + page + "&status=" + ResultStatus.success + "&MsgError=Success");
        }
      
        public JsonResult GetInvoicesNotReturn(string txtSearch = "", long TypeId = 0, long InvId = 0, int page = 1, int pageSize = 10)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();

           
            var itemsList = new InvoiceService().GetInvoicesNotReturn(txtSearch, TypeId, InvId, page, pageSize);
            var list = itemsList.Distinct().OrderBy(_ => _.Code)
                .Select(_ => new
                {
                    _.Id,
                    _.Code                    
                })
                .ToList();
            return Json(list);
        }       

        public ActionResult SearchInvoices(string txt = "" , long dealerId = 0 , long currencyId =0, int page = 1 , long typeId = 1, int Type = 1, int index = 0 , string ids = "")
        {
            ViewBag.index = index;
            ViewBag.dealerId = dealerId;
            ViewBag.currencyId = currencyId;
            ViewBag.Type = Type;          
            ViewBag.ids = ids;            
            var list = new InvoiceService().GetCreditAllByDealerId(txt , dealerId, currencyId , ids, 0 , typeId,  page, 10);
            return Type != 1 ? (ActionResult)PartialView("SearchInvoicesList", list) : View("SearchInvoices", list);
        }
       
        public JsonResult checkStock(int id)
        {
            var invoice = new InvoiceService().Get(id);
            var data = new
            {
                code = invoice.Code,
                dealer = invoice.DealerName,
                date = invoice.Date,
                net = invoice.Net,
                credit = invoice.Credit
            };
            return Json(data);
        }
    }
}