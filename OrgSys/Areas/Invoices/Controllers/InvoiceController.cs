using Entity.ModelView;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using Microsoft.Extensions.Configuration;
using OrgSys.Controllers;
using Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace OrgSys.Areas.Invoices.Controllers
{
    [Area("Invoices")]
    public class InvoiceController(IConfiguration configuration) : MainController<InvoiceModelView>(configuration)
    {
        public override async Task LoadViewBagIndex(long ParentId = 0, long TypeId = 0)
        {
            var type = await GetObApi<InvoiceTypeModelView>(Domain.Enums.ApiMethodType.Get, $"GetById?Id={TypeId}");
            ViewBag.InvoicesType = type.Name;
            ViewBag.InvoicesGroup = type.Group;
            ViewBag.InvoicesIcon = type.Icon;

            await base.LoadViewBagIndex();
        }

        public override async Task LoadViewBag(InvoiceModelView model)
        {
            ViewBag.CurrencyId = new SelectList(await GetListApi<CurrencyModelView>(Domain.Enums.ApiMethodType.Get, $"GetList?KeySearch=&Page=1&PageSize=20"), "Id", "Name", model.CurrencyId);
            ViewBag.PaymentTypeId = new SelectList(await GetListApi<PaymentTypeModelView>(Domain.Enums.ApiMethodType.Get, $"GetList?KeySearch=&Page=1&PageSize=20"), "Id", "Name", model.PaymentTypeId);

            List<SelectListItem> selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem { Value = "1", Text = Translate.GetTranslate("Amount") });
            selectListItems.Add(new SelectListItem { Value = "2", Text = Translate.GetTranslate("Ratio") });

            ViewBag.DiscountType = new SelectList(selectListItems, "Value", "Text");
            ViewBag.ServiceType = new SelectList(selectListItems, "Value", "Text");
            ViewBag.TaxType = new SelectList(selectListItems, "Value", "Text");

            var type = await GetObApi<InvoiceTypeModelView>(Domain.Enums.ApiMethodType.Get, $"GetById?Id={model.TypeId}");
            ViewBag.InvoicesType = type.Name;
            ViewBag.InvoicesGroup = type.Group;
            ViewBag.InvoicesIcon = type.Icon;
        }

        public override async Task<InvoiceModelView> InitializeData(InvoiceModelView ob)
        {
            var preferenceList = await GetListApi<PreferenceModelView>(Domain.Enums.ApiMethodType.Get, $"GetList?KeySearch=Invoice&TypeId={ob.TypeId}&Page=1&PageSize=1000");
            var StockId = long.Parse("0" + preferenceList.FirstOrDefault(e => e.Key == "DefaultStock")?.Value);

            long DealerId = 0;
            if (ob.TypeId == 1 || ob.TypeId == 3)
                DealerId = long.Parse("0" + preferenceList.FirstOrDefault(e => e.Key == "DefaultCustomer")?.Value);
            else if (ob.TypeId == 2 || ob.TypeId == 4)
                DealerId = long.Parse("0" + preferenceList.FirstOrDefault(e => e.Key == "DefaultSupplier")?.Value);

            var PaymentTypeId = long.Parse("0" + preferenceList.FirstOrDefault(e => e.Key == "DefaultPaymentType")?.Value);
            var DefaultCurrencyId = long.Parse("0" + preferenceList.FirstOrDefault(e => e.Key == "DefaultCurrency")?.Value);
            var DefaultDiscountType = int.Parse("0" + preferenceList.FirstOrDefault(e => e.Key == "DefaultDiscountType")?.Value);
            var DefaultServiceType = int.Parse("0" + preferenceList.FirstOrDefault(e => e.Key == "DefaultServiceType")?.Value);
            var DefaultTaxType = int.Parse("0" + preferenceList.FirstOrDefault(e => e.Key == "DefaultTaxType")?.Value);
            var DiscountValue = decimal.Parse("0" + preferenceList.FirstOrDefault(e => e.Key == "DiscountValue")?.Value);
            var ServiceValue = decimal.Parse("0" + preferenceList.FirstOrDefault(e => e.Key == "ServiceValue")?.Value);
            var TaxValue = decimal.Parse("0" + preferenceList.FirstOrDefault(e => e.Key == "TaxValue")?.Value);
            ViewBag.NumberLine = int.Parse("0" + preferenceList.FirstOrDefault(e => e.Key == "NumberLine")?.Value);
            ViewBag.OrderTabe = int.Parse("0" + preferenceList.FirstOrDefault(e => e.Key == "OrderTabe")?.Value);
            ViewBag.AutoSave = int.Parse("0" + preferenceList.FirstOrDefault(e => e.Key == "AutoSave")?.Value);
            var TypeCode = int.Parse("0" + preferenceList.FirstOrDefault(e => e.Key == "TypeSerial")?.Value);
            ViewBag.TypeSerial = TypeCode;
            ViewBag.AllowRepeated = int.Parse("0" + preferenceList.FirstOrDefault(e => e.Key == "AllowRepeated")?.Value);

            if (ob == null)
                ob = new InvoiceModelView();

            if (ob.Id == 0)
            {
                ob.CodeNumber = long.Parse("0" + await GetValueApi<InvoiceModelView>(Domain.Enums.ApiMethodType.Get, $"GetMax?TypeId={ob.TypeId}")) + 1;
                ob.Code = "" + ob.CodeNumber;
                ob.StockId = StockId;
                ob.DealerId = DealerId;
                ob.PaymentTypeId = PaymentTypeId;
                ob.CurrencyId = DefaultCurrencyId;
                ob.Date = DateTime.Now;
                ob.DiscountType = DefaultDiscountType;
                ob.ServiceType = DefaultServiceType;
                ob.TaxType = DefaultTaxType;
                ob.Discount = DiscountValue;
                ob.Service = ServiceValue;
                ob.Tax = TaxValue;
                ob.InvoiceProductList = new List<InvoiceProductModelView>();
            }

            ob.StockName = (await GetObApi<StockModelView>(Domain.Enums.ApiMethodType.Get, $"GetById?Id={ob.StockId ?? 0}"))?.Name;
            ob.DealerName = (await GetObApi<DealerModelView>(Domain.Enums.ApiMethodType.Get, $"GetById?Id={ob.DealerId}"))?.Name;
            ob.ParentCode = (await GetObApi<InvoiceModelView>(Domain.Enums.ApiMethodType.Get, $"GetById?Id={ob.StockId ?? 0}"))?.Code;
            ob.Rate = (await GetObApi<CurrencyModelView>(Domain.Enums.ApiMethodType.Get, $"GetById?Id={ob.StockId ?? 0}"))?.Rate ?? 0;
            return ob;
        }

        public override Task<InvoiceModelView> FixData(InvoiceModelView ob)
        {
            if (ob.Id == 0)
            {
                ob.CreateUserId = User.GetUserId();
                ob.CreateDate = DateTime.Now;
            }
            else
            {
                ob.ModifyUserId = User.GetUserId();
                ob.ModifyDate = DateTime.Now;
            }
            return base.FixData(ob);
        }

        public ActionResult CreateTransaction(long id, string search, long ParentId = 0, long TypeId = 0, int page = 1)
        {
            new IntegrationServics(User.GetSchema()).CreateTransactionByInvoice(new InvoiceService(User.GetSchema()).Get(id));
            return Redirect("/Invoices/Invoice/Index?ParentId=" + ParentId + "&TypeId=" + TypeId + "&page=" + page + "&status=" + ResultStatus.success + "&MsgError=Success");
        }

        public ActionResult CreateFinancial(long id, string search, long ParentId = 0, long TypeId = 0, int page = 1, string dir = "Index")
        {
            new IntegrationServics(User.GetSchema()).CollectPaidInvoice(new InvoiceService(User.GetSchema()).Get(id));
            if (dir == "Index")
                return Redirect("/Invoices/Invoice/Index?ParentId=" + ParentId + "&TypeId=" + TypeId + "&page=" + page + "&status=" + ResultStatus.success + "&MsgError=Success");
            else
                return Redirect("/Invoices/Invoice/Save?Id=" + id + "&ParentId=" + ParentId + "&TypeId=" + TypeId + "&page=" + page + "&status=" + ResultStatus.success + "&MsgError=Success");
        }

        public JsonResult CollectInvoice(long id)
        {
            new IntegrationServics(User.GetSchema()).CreateFinancialByInvoice(new InvoiceService(User.GetSchema()).Get(id));
            return Json("Ok");
        }

        public JsonResult GetInvoicesNotReturn(string txtSearch = "", long TypeId = 0, long InvId = 0, int page = 1, int pageSize = 10)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();

            var itemsList = new InvoiceService(User.GetSchema()).GetInvoicesNotReturn(txtSearch, TypeId, InvId, page, pageSize);
            var list = itemsList.Distinct().OrderBy(_ => _.Code)
                .Select(_ => new
                {
                    _.Id,
                    _.Code
                })
                .ToList();
            return Json(list);
        }

        public ActionResult SearchInvoices(string txt = "", long dealerId = 0, long currencyId = 0, int page = 1, long typeId = 1, int Type = 1, int index = 0, string ids = "")
        {
            ViewBag.index = index;
            ViewBag.dealerId = dealerId;
            ViewBag.currencyId = currencyId;
            ViewBag.Type = Type;
            ViewBag.ids = ids;
            var list = new InvoiceService(User.GetSchema()).GetCreditAllByDealerId(txt, dealerId, currencyId, ids, 0, typeId, page, 10);
            return Type != 1 ? (ActionResult)PartialView("SearchInvoicesList", list) : View("SearchInvoices", list);
        }

        public async Task<JsonResult> checkStock(int id)
        {
            var invoice = await GetObApi<InvoiceModelView>(Domain.Enums.ApiMethodType.Get, $"GetById?Id={id}");
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

        public ActionResult Cancel(long id, string search, long ParentId = 0, long TypeId = 0, int page = 1)
        {
            new InvoiceService(User.GetSchema()).Cancel(id);
            return Redirect("/Invoices/Invoice/Index?ParentId=" + ParentId + "&TypeId=" + TypeId + "&page=" + page + "&status=" + ResultStatus.success + "&MsgError=Success");
        }

        public ActionResult Redo(long id, string search, long ParentId = 0, long TypeId = 0, int page = 1)
        {
            new InvoiceService(User.GetSchema()).Redo(id);
            return Redirect("/Invoices/Invoice/Index?ParentId=" + ParentId + "&TypeId=" + TypeId + "&page=" + page + "&status=" + ResultStatus.success + "&MsgError=Success");
        }

        //public override Task<IActionResult> Print(long Id, string ViewName = "InvoicePrint")
        //{
        //    return base.Print(Id, ViewName);
        //}

        public JsonResult GetProductInvoice(int Id)
        {

            var item = new InvoiceService(User.GetSchema()).GetProductInvoicesNotReturn(Id);
            if (item == null)
                item = new List<InvoiceProductModelView>();
            var data = item.Select(e => new
            {
                productId = e.ProductId,
                quantity = e.Quantity
            }).ToList();
            return Json(data);
        }
    }
}