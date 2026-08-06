using Application.Commands.Org.Invoices.Invoice.Commands;
using AutoMapper;
using Domain.Enums;
using Domain.Shared;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OrgSys.Controllers;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgSys.Areas.Invoices.Controllers
{
    [Area("Invoices")]
    public class InvoiceController(IConfiguration configuration, IMapper mapper) : MainController<InvoiceDto, CreateInvoiceCommand, UpdateInvoiceCommand>(configuration, mapper)
    {
        public override async Task LoadViewBagIndex(long ParentId = 0, long TypeId = 0)
        {
            var type = await GetObApi<InvoiceTypeDto>($"GetById?Id={TypeId}");
            ViewBag.InvoicesType = type.Name;
            ViewBag.InvoicesGroup = type.Group;
            ViewBag.InvoicesIcon = type.Icon;
        }

        public override async Task LoadViewBag(InvoiceDto model)
        {
            ViewBag.CurrencyId = new SelectList(await GetListApi<CurrencyDto>(), "Id", "Name", model.CurrencyId);
            ViewBag.PaymentTypeId = new SelectList(await GetListApi<PaymentTypeDto>(), "Id", "Name", model.PaymentTypeId);

            List<SelectListItem> selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem { Value = "1", Text = Domain.Resource.Translate.GetTranslate("Amount") });
            selectListItems.Add(new SelectListItem { Value = "2", Text = Domain.Resource.Translate.GetTranslate("Ratio") });

            ViewBag.DiscountType = new SelectList(selectListItems, "Value", "Text");
            ViewBag.ServiceType = new SelectList(selectListItems, "Value", "Text");
            ViewBag.TaxType = new SelectList(selectListItems, "Value", "Text");

            var type = await GetObApi<InvoiceTypeDto>($"GetById?Id={model.TypeId}");
            ViewBag.InvoicesType = type.Name;
            ViewBag.InvoicesGroup = type.Group;
            ViewBag.InvoicesIcon = type.Icon;
        }

        public override async Task<InvoiceDto> InitializeData(InvoiceDto ob)
        {
            var preferenceList = await GetListApi<PreferenceDto>(TypeId: ob.TypeId, TextSearch: "Invoice", PageSize: 1000);
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
                ob = new InvoiceDto();

            if (ob.Id == 0)
            {

                ob.CodeNumber = long.Parse("0" + await GetValueApi<InvoiceDto>($"GetMax?ParentId=0&TypeId={ob.TypeId}")) + 1;
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
                ob.InvoiceProductList = new List<InvoiceProductDto>();
            }

            ob.StockName = (await GetObApi<StockDto>($"GetById?Id={ob.StockId ?? 0}"))?.Name;
            ob.DealerName = (await GetObApi<DealerDto>($"GetById?Id={ob.DealerId}"))?.Name;
            ob.ParentCode = (await GetObApi<InvoiceDto>($"GetById?Id={ob.StockId ?? 0}"))?.Code;
            ob.Rate = (await GetObApi<CurrencyDto>($"GetById?Id={ob.CurrencyId}"))?.Rate ?? 0;
            return ob;
        }

        public override Task<InvoiceDto> FixData(InvoiceDto ob)
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

        public async Task<ActionResult> CreateTransaction(long id, long ParentId = 0, long TypeId = 0, int page = 1, string dir = "Index")
        {
            var response = await ApiMethod(ApiMethodType.Post, $"CreateTransactionInvoice?InvoiceId={id}");
            //if (dir == "Save")
            //    return Redirect($"/Invoices/Invoice/Save?id={id}&ParentId={ParentId}&TypeId={TypeId}&status={(response.IsSuccessStatusCode ? ResultStatus.success : ResultStatus.error)}&MsgError={(response.IsSuccessStatusCode ? "Success" : "Unable to create transaction")}");
            return Redirect($"/Invoices/Invoice/Index?ParentId={ParentId}&TypeId={TypeId}&page={page}&status={(response.IsSuccessStatusCode ? ResultStatus.success : ResultStatus.error)}&MsgError={(response.IsSuccessStatusCode ? "Success" : "Unable to create transaction")}");
        }

        public async Task<ActionResult> CreateJournal(long id, long ParentId = 0, long TypeId = 0, int page = 1, string dir = "Index")
        {
            var response = await ApiMethod(ApiMethodType.Post, $"CreateJournal?InvoiceId={id}");
            //if (dir == "Save")
            //    return Redirect($"/Invoices/Invoice/Save?id={id}&ParentId={ParentId}&TypeId={TypeId}&status={(response.IsSuccessStatusCode ? ResultStatus.success : ResultStatus.error)}&MsgError={(response.IsSuccessStatusCode ? "Success" : "Unable to create journal. Check account integration settings.")}");
            return Redirect($"/Invoices/Invoice/Index?ParentId={ParentId}&TypeId={TypeId}&page={page}&status={(response.IsSuccessStatusCode ? ResultStatus.success : ResultStatus.error)}&MsgError={(response.IsSuccessStatusCode ? "Success" : "Unable to create journal. Check account integration settings.")}");
        }

        public async Task<ActionResult> CreateFinancial(long id, string search, long ParentId = 0, long TypeId = 0, int page = 1, string dir = "Index")
        {
            //var invoice = await GetObApi<InvoiceModelView>($"GetById?Id={id}");
            var response = await ApiMethod(ApiMethodType.Post, $"CollectPaidInvoice?InvoiceId={id}");
            //var x = new InvoiceService(User.GetSchema()).Get(id);
            //new IntegrationServics(User.GetSchema()).CollectPaidInvoice(invoice);
            if (dir == "Index")
                return Redirect("/Invoices/Invoice/Index?ParentId=" + ParentId + "&TypeId=" + TypeId + "&page=" + page + "&status=" + ResultStatus.success + "&MsgError=Success");
            else
                return Redirect("/Invoices/Invoice/Save?Id=" + id + "&ParentId=" + ParentId + "&TypeId=" + TypeId + "&page=" + page + "&status=" + ResultStatus.success + "&MsgError=Success");
        }

        //This end point is used from reports
        //public JsonResult CollectInvoice(long id)
        //{
        //    new IntegrationServics(User.GetSchema()).CreateFinancialByInvoice(new InvoiceService(User.GetSchema()).Get(id));
        //    return Json("Ok");
        //}

        public async Task<JsonResult> GetInvoicesNotReturn(string txtSearch = "", long TypeId = 0, long InvId = 0, int page = 1, int pageSize = 10)
        {

            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();

            var items = await GetListApi<InvoiceDto>($"GetInvoicesNotReturn?KeySearch={txtSearch}&ParentId={InvId}&TypeId={TypeId - 2}&Page={page}&PageSize={pageSize}");

            var lsit = items.Distinct().OrderBy(_ => _.Code).Select(_ => new { _.Id, _.Code }).ToList();

            return Json(lsit);
        }

        public async Task<ActionResult> SearchInvoices(string txt = "", long dealerId = 0,
            long currencyId = 0, int page = 1, long typeId = 1, int Type = 1, int index = 0, string ids = "")
        {
            ViewBag.index = index;
            ViewBag.dealerId = dealerId;
            ViewBag.currencyId = currencyId;
            ViewBag.Type = Type;
            ViewBag.ids = ids;
            var response = await ApiMethod(ApiMethodType.Get, $"SearchInvoice?KeySearch={txt}&dealerId={dealerId}&currencyId={currencyId}&typeId={typeId}&Page={page}&PageSize=10&Ids={ids}");

            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            var dataList = JsonConvert.DeserializeObject<ResultPagination<InvoiceDto>>(data);
            return Type != 1 ? (ActionResult)PartialView("SearchInvoicesList", dataList) : View("SearchInvoices", dataList);
        }

        public async Task<JsonResult> checkStock(int id)
        {
            var invoice = await GetObApi<InvoiceDto>($"GetById?Id={id}");
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
            var response = await ApiMethod(ApiMethodType.Put, $"Cancel?Id={id}");
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

        public async Task<JsonResult> GetProductInvoice(int Id)
        {

          var responseMessage =  ApiMethod(ApiMethodType.Get, $"GetProductInvoicesNotReturn?Id={Id}").Result.EnsureSuccessStatusCode();

            responseMessage.EnsureSuccessStatusCode();
            var dataa = await responseMessage.Content.ReadAsStringAsync();

            var item = JsonConvert.DeserializeObject<ResultCollection<InvoiceProductDto>>(dataa);

            var data = item.Response.Select(e => new
            {
                productId = e.ProductId,
                quantity = e.Quantity
            }).ToList();
            return Json(data);
        }

        [HttpPost]
        public async Task<ActionResult> AutoSave(InvoiceDto ob)
        {
            await base.Save(ob);
            if (ob.Id == 0)
            {

                var key = ob.GetType().GetProperty("Code")?.GetValue(ob, null);
                var searchResp = await ApiMethod(ApiMethodType.Get, $"Search?KeySearch={key}&ParentId={ob.ParentId}&TypeId={ob.TypeId}&Page=1&PageSize=1");
                if (searchResp != null && searchResp.IsSuccessStatusCode)
                {
                    var searchData = await searchResp.Content.ReadAsStringAsync();
                    var searchRes = JsonConvert.DeserializeObject<ResultPagination<InvoiceDto>>(searchData);
                    if (searchRes != null && searchRes.Response != null && searchRes.Response.Count > 0)
                    {
                        ob.Id = searchRes.Response[0].Id;
                        ob.CreateUserId = searchRes.Response[0].CreateUserId;
                    }

                    return Ok(new
                    {
                        status = "success",
                        id = ob.Id,
                        createdUserId = ob.CreateUserId,
                        url = "/" + "Invoices" + "/" + "Invoice" + "?ParentId=" + ob.ParentId + "&TypeId=" + ob.TypeId + "&status=" + ResultStatus.success + "&MsgError=Success"
                    });
                }
            }
            return Ok();
        }
    }
}
