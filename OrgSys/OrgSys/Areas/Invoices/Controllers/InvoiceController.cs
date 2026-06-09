using Application.Commands.Org.Invoices.Invoice.Commands;
using AutoMapper;
using Azure;
using Domain.Enums;
using Domain.Shared;
using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OrgSys.Controllers;
using PuppeteerSharp;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace OrgSys.Areas.Invoices.Controllers
{
    [Area("Invoices")]
    public class InvoiceController(IConfiguration configuration, IMapper mapper) : MainController<InvoiceModelView, CreateInvoiceCommand, UpdateInvoiceCommand>(configuration, mapper)
    {
        public override async Task LoadViewBagIndex(long ParentId = 0, long TypeId = 0)
        {
            var type = await GetObApi<InvoiceTypeModelView>($"GetById?Id={TypeId}");
            ViewBag.InvoicesType = type.Name;
            ViewBag.InvoicesGroup = type.Group;
            ViewBag.InvoicesIcon = type.Icon;             
        }

        public override async Task LoadViewBag(InvoiceModelView model)
        {
            ViewBag.CurrencyId = new SelectList(await GetListApi<CurrencyModelView>(), "Id", "Name", model.CurrencyId);
            ViewBag.PaymentTypeId = new SelectList(await GetListApi<PaymentTypeModelView>(), "Id", "Name", model.PaymentTypeId);

            List<SelectListItem> selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem { Value = "1", Text = Translate.GetTranslate("Amount") });
            selectListItems.Add(new SelectListItem { Value = "2", Text = Translate.GetTranslate("Ratio") });

            ViewBag.DiscountType = new SelectList(selectListItems, "Value", "Text");
            ViewBag.ServiceType = new SelectList(selectListItems, "Value", "Text");
            ViewBag.TaxType = new SelectList(selectListItems, "Value", "Text");

            var type = await GetObApi<InvoiceTypeModelView>($"GetById?Id={model.TypeId}");
            ViewBag.InvoicesType = type.Name;
            ViewBag.InvoicesGroup = type.Group;
            ViewBag.InvoicesIcon = type.Icon;
        }

        public override async Task<InvoiceModelView> InitializeData(InvoiceModelView ob)
        {
            var preferenceList = await GetListApi<PreferenceModelView>(TypeId: ob.TypeId, PageSize: 1000);
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
                ob.CodeNumber = long.Parse("0" + await GetValueApi<InvoiceModelView>($"GetMax?TypeId={ob.TypeId}")) + 1;
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

            ob.StockName = (await GetObApi<StockModelView>($"GetById?Id={ob.StockId ?? 0}"))?.Name;
            ob.DealerName = (await GetObApi<DealerModelView>($"GetById?Id={ob.DealerId}"))?.Name;
            ob.ParentCode = (await GetObApi<InvoiceModelView>($"GetById?Id={ob.StockId ?? 0}"))?.Code;
            ob.Rate = (await GetObApi<CurrencyModelView>($"GetById?Id={ob.StockId ?? 0}"))?.Rate ?? 0;
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

        public async Task<ActionResult> SearchInvoices(string txt = "", long dealerId = 0,
            long currencyId = 0, int page = 1, long typeId = 1, int Type = 1, int index = 0, string ids = "")
        {
            ViewBag.index = index;
            ViewBag.dealerId = dealerId;
            ViewBag.currencyId = currencyId;
            ViewBag.Type = Type;
            ViewBag.ids = ids;
            //var list1 = new InvoiceService(User.GetSchema()).GetCreditAllByDealerId(txt, dealerId, currencyId, ids, 0, typeId, page, 10);
            var response = await ApiMethod(ApiMethodType.Get ,$"SearchInvoice?KeySearch={txt}&dealerId={dealerId}&currencyId={currencyId}&typeId={typeId}&Page={page}&PageSize=10&Ids={ids}");

            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            var dataList = JsonConvert.DeserializeObject<ResultPagination<InvoiceModelView>>(data);
            return Type != 1 ? (ActionResult)PartialView("SearchInvoicesList", dataList) : View("SearchInvoices", dataList);
        }

        public async Task<JsonResult> checkStock(int id)
        {
            var invoice = await GetObApi<InvoiceModelView>($"GetById?Id={id}");
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

        public async Task<ActionResult> Cancel(long id, string search, long ParentId = 0, long TypeId = 0, int page = 1)
        {
         var response =    await ApiMethod(ApiMethodType.Put , $"Cancel?Id={id}");
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<Domain.Shared.Result>(data);

            return Redirect("/Invoices/Invoice/Index?ParentId=" + ParentId + "&TypeId=" + TypeId + "&page=" + page + "&status=" + (res.StatusCode != null ? ResultStatus.success : ResultStatus.error) + "&MsgError=Success");
        }

        public async Task<ActionResult> Redo(long id, string search, long ParentId = 0, long TypeId = 0, int page = 1)
        {
            var response = await ApiMethod(ApiMethodType.Put, $"Redo?Id={id}");
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<Domain.Shared.Result>(data);

            return Redirect("/Invoices/Invoice/Index?ParentId=" + ParentId + "&TypeId=" + TypeId + "&page=" + page + "&status=" + (res.StatusCode != null ? ResultStatus.success : ResultStatus.error) + "&MsgError=Success");
        }

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