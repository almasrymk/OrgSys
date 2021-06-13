using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrgSys.Controllers;
using Service.BAL;
using System;
using System.Collections.Generic;
using Utility;

namespace OrgSys.Areas.Orders.Controllers
{
    [Area("Orders")]
    public class OrderController : BaseController<OrderModelView>
    {
        public override void LoadViewBagIndex(long ParentId = 0, long TypeId = 0)
        {
            var type = new OrderTypeService().Get(TypeId);
            ViewBag.OrdersType = type.Name;
            ViewBag.OrdersIcon = type.Icon;

            base.LoadViewBagIndex();
        }

        public override void LoadViewBag(OrderModelView model)
        {
            ViewBag.DealerId = new SelectList(new DealerService().GetAll(model.ParentId, model.TypeId == 1 || model.TypeId == 3 ? (int)DealerType.Client : (int)DealerType.Supplier), "Id", "Name", model.DealerId);
            ViewBag.ProductId = new SelectList(new ProductService().GetAll(model.ParentId, 0), "Id", "Name");
            ViewBag.TableId = new SelectList(new TableService().GetAll(model.ParentId, 0), "Id", "Name");

            List<SelectListItem> selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem { Value = "1", Text = "Amount" });
            selectListItems.Add(new SelectListItem { Value = "2", Text = "Percentage" });

            ViewBag.DiscountType = new SelectList(selectListItems, "Value", "Text");
            ViewBag.ServiceType = new SelectList(selectListItems, "Value", "Text");
            ViewBag.TaxType = new SelectList(selectListItems, "Value", "Text");

            var type = new OrderTypeService().Get(model.TypeId);
            ViewBag.OrdersType = type.Name;
            ViewBag.OrdersIcon = type.Icon;
        }

        public override OrderModelView InitializeData(OrderModelView ob)
        {
            var setting = new PreferenceService();

            long DealerId = 0;
            if (ob.TypeId == 1 || ob.TypeId == 3)
                DealerId = long.Parse("0" + setting.GetByKey("DefaultCustomer", "Order", ob.TypeId, 0)?.Value);
            else if (ob.TypeId == 2 || ob.TypeId == 4)
                DealerId = long.Parse("0" + setting.GetByKey("DefaultSupplier", "Order", ob.TypeId, 0)?.Value);

            var DefaultDiscountType = int.Parse("0" + setting.GetByKey("DefaultDiscountType", "Order", ob.TypeId, 0)?.Value);
            var DefaultServiceType = int.Parse("0" + setting.GetByKey("DefaultServiceType", "Order", ob.TypeId, 0)?.Value);
            var DefaultTaxType = int.Parse("0" + setting.GetByKey("DefaultTaxType", "Order", ob.TypeId, 0)?.Value);
            var DiscountValue = decimal.Parse("0" + setting.GetByKey("DiscountValue", "Order", ob.TypeId, 0)?.Value);
            var ServiceValue = decimal.Parse("0" + setting.GetByKey("ServiceValue", "Order", ob.TypeId, 0)?.Value);
            var TaxValue = decimal.Parse("0" + setting.GetByKey("TaxValue", "Order", ob.TypeId, 0)?.Value);
            ViewBag.NumberLine = int.Parse("0" + setting.GetByKey("NumberLine", "Order", ob.TypeId, 0)?.Value);
            ViewBag.OrderTabe = int.Parse("0" + setting.GetByKey("OrderTabe", "Order", ob.TypeId, 0)?.Value);
            ViewBag.AutoSave = int.Parse("0" + setting.GetByKey("AutoSave", "Order", ob.TypeId, 0)?.Value);
            var TypeCode = int.Parse("0" + setting.GetByKey("TypeSerial", "Order", ob.TypeId, 0)?.Value);
            ViewBag.TypeSerial = TypeCode;
            ViewBag.AllowRepeated = int.Parse("0" + setting.GetByKey("AllowRepeated", "Order", ob.TypeId, 0)?.Value);

            if (ob == null)
                ob = new OrderModelView();

            if (ob.Id == 0)
            {
                ob.CodeNumber = new OrderService().GetMaxCode(ob.TypeId);
                ob.Code = "" + new OrderService().GetMaxCode(ob.TypeId);
                ob.DealerId = DealerId;
                ob.Date = DateTime.Now;
                ob.DiscountType = DefaultDiscountType;
                ob.ServiceType = DefaultServiceType;
                ob.TaxType = DefaultTaxType;
                ob.Discount = DiscountValue;
                ob.Service = ServiceValue;
                ob.Tax = TaxValue;
                ob.OrderProducts = new List<OrderProductModelView>();
              }           
            return ob;
        }       
    }
}