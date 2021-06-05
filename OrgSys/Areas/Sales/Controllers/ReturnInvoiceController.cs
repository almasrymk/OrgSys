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

namespace OrgSys.Areas.Sales.Controllers
{
    [Area("Sales")]
    public class ReturnInvoiceController : BaseController<InvoiceModelView>
    {
        public override void LoadViewBag(InvoiceModelView model)
        {
            ViewBag.DealerId = new SelectList(new DealerService().GetAll(model.ParentId, (int)DealerType.Client), "Id", "Name", model.DealerId);
            ViewBag.StoreId = new SelectList(new StoreService().GetAll(model.ParentId, model.StoreId), "Id", "Name", model.StoreId);
            ViewBag.PaymentTypeId = new SelectList(new PaymentTypeService().GetAll(model.ParentId, model.PaymentTypeId), "Id", "Name", model.PaymentTypeId);
            ViewBag.ProductId = new SelectList(new ProductService().GetAll(model.ParentId, 0), "Id", "Name");
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem { Value = "1", Text = "Amount" });
            selectListItems.Add(new SelectListItem { Value = "2", Text = "Percentage" });

            ViewBag.DiscountType = new SelectList(selectListItems, "Value", "Text");
            ViewBag.ServiceType = new SelectList(selectListItems, "Value", "Text");
            ViewBag.TaxType = new SelectList(selectListItems, "Value", "Text");
        }

        public override InvoiceModelView InitializeData(InvoiceModelView ob)
        {
            var setting = new PreferenceService();
            var StoreId = long.Parse("0" + setting.GetByKey("DefaultStore", "Invoice", 3, 0)?.Value);
            var CustomerId = long.Parse("0" + setting.GetByKey("DefaultCustomer", "Invoice", 3, 0)?.Value);
            var PaymentTypeId = long.Parse("0" + setting.GetByKey("DefaultPaymentType", "Invoice", 3, 0)?.Value);
            var DefaultDiscountType = int.Parse("0" + setting.GetByKey("DefaultDiscountType", "Invoice", 3, 0)?.Value);
            var DefaultServiceType = int.Parse("0" + setting.GetByKey("DefaultServiceType", "Invoice", 3, 0)?.Value);
            var DefaultTaxType = int.Parse("0" + setting.GetByKey("DefaultTaxType", "Invoice", 3, 0)?.Value);
            var DiscountValue = decimal.Parse("0" + setting.GetByKey("DiscountValue", "Invoice", 3, 0)?.Value);
            var ServiceValue = decimal.Parse("0" + setting.GetByKey("ServiceValue", "Invoice", 3, 0)?.Value);
            var TaxValue = decimal.Parse("0" + setting.GetByKey("TaxValue", "Invoice", 3, 0)?.Value);
            ViewBag.NumberLine = int.Parse("0" + setting.GetByKey("NumberLine", "Invoice", 3, 0)?.Value);
            ViewBag.OrderTabe = int.Parse("0" + setting.GetByKey("OrderTabe", "Invoice", 3, 0)?.Value);
            ViewBag.AutoSave = int.Parse("0" + setting.GetByKey("AutoSave", "Invoice",3, 0)?.Value);
            var TypeCode = int.Parse("0" + setting.GetByKey("TypeSerial", "Invoice", 3, 0)?.Value);
            ViewBag.TypeSerial = TypeCode;
            ViewBag.AllowRepeated = int.Parse("0" + setting.GetByKey("AllowRepeated", "Invoice", 3, 0)?.Value);

            if (ob == null)
                ob = new InvoiceModelView();

            if (ob.Id == 0)
            {
                ob.CodeNumber = new InvoiceService().GetMaxCode(3);
                ob.Code = "" + new InvoiceService().GetMaxCode(3);
                ob.StoreId = StoreId;
                ob.DealerId = CustomerId;
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

            return ob;
        }
    }
}
