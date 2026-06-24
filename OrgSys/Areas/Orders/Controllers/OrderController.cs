using Entity;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrgSys.Controllers;
using Service;
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
            //var type = new OrderTypeService(User.GetSchema()).Get(TypeId);
            //ViewBag.OrdersType = type.Name;
            //ViewBag.OrdersIcon = type.Icon;

            base.LoadViewBagIndex();
        }

        public override void LoadViewBag(OrderModelView model)
        {
            ViewBag.TableId = new SelectList(new TableService(User.GetSchema()).GetAllClosed(model.Id, model.ParentId, 0), "Id", "Name", model.TableId);

            List<SelectListItem> selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem { Value = "1", Text = Translate.GetTranslate("Amount") });
            selectListItems.Add(new SelectListItem { Value = "2", Text = Translate.GetTranslate("Ratio") });

            ViewBag.DiscountType = new SelectList(selectListItems, "Value", "Text");
            ViewBag.ServiceType = new SelectList(selectListItems, "Value", "Text");
            ViewBag.TaxType = new SelectList(selectListItems, "Value", "Text");

            //var type = new OrderTypeService(User.GetSchema()).Get(model.TypeId);
            //ViewBag.OrdersType = type.Name;
            //ViewBag.OrdersIcon = type.Icon;
        }

        public override OrderModelView InitializeData(OrderModelView ob)
        {
            var setting = new PreferenceService(User.GetSchema());

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
               // ob.CodeNumber = new OrderService(User.GetSchema()).GetMaxCode(ob.TypeId);
                ob.Code = "" + ob.CodeNumber;
                ob.DealerId = DealerId;
                ob.Date = DateTime.Now;
                ob.DiscountType = DefaultDiscountType;
                ob.ServiceType = DefaultServiceType;
                ob.TaxType = DefaultTaxType;
                ob.Discount = DiscountValue;
                ob.Service = ServiceValue;
                ob.Tax = TaxValue;
                ob.OrderProductList = new List<OrderProductModelView>();
            }

            ob.DealerName = new DealerService(User.GetSchema()).Get(ob.DealerId ?? 0).Name;
            return ob;
        }

        public ActionResult CreateInvoice(long id, string search, long ParentId = 0, long TypeId = 0, int page = 1)
        {
            //var order = new OrderService(User.GetSchema()).Get(id);
            //if (order != null)
            //{
            //    new IntegrationServics(User.GetSchema()).CreateInvoiceByOrder(order.Map<OrderModelView>());
            //    return Redirect("/Orders/Order/Index?ParentId=" + ParentId + "&TypeId=" + TypeId + "&page=" + page + "&status=" + ResultStatus.success + "&MsgError=Success");
            //}
            return Redirect("/Orders/Order/Index?ParentId=" + ParentId + "&TypeId=" + TypeId + "&page=" + page + "&status=" + ResultStatus.error + "&MsgError=Not find order");
        }

        public ActionResult Cancel(long id, string search, long ParentId = 0, long TypeId = 0, int page = 1)
        {
            //new OrderService(User.GetSchema()).Cancel(id);
            return Redirect("/Orders/Order/Index?ParentId=" + ParentId + "&TypeId=" + TypeId + "&page=" + page + "&status=" + ResultStatus.success + "&MsgError=Success");
        }

        public ActionResult Redo(long id, string search, long ParentId = 0, long TypeId = 0, int page = 1)
        {
            //new OrderService(User.GetSchema()).Redo(id);
            return Redirect("/Orders/Order/Index?ParentId=" + ParentId + "&TypeId=" + TypeId + "&page=" + page + "&status=" + ResultStatus.success + "&MsgError=Success");
        }
    }
}