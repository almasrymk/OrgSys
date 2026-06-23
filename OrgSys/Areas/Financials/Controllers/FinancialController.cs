using Application.Commands.Org.Financials.Financial.Commands;
using AutoMapper;
using Domain.Enums;
using Domain.Shared;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OrgSys.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Utility;

namespace OrgSys.Areas.Financial.Controllers
{
    [Area("Financials")]
    public class FinancialController(IConfiguration configuration, IMapper mapper) :
        MainController<FinancialDto, CreateFinancialCommand , UpdateFinancialCommand>(configuration , mapper)
    {
        public override async Task LoadViewBagIndex(long ParentId = 0, long TypeId = 0)
        {

            //var type = new FinancialTypeService(User.GetSchema()).Get(TypeId);
            var type = await GetObApi<FinancialTypeDto>($"GetById?Id={TypeId}");
            ViewBag.FinancialsType = type.Name;
            ViewBag.FinancialsIcon = type.Icon;
        }

        public override Task<FinancialDto> FixData(FinancialDto ob)
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
        public override async Task LoadViewBag(FinancialDto model)
        {

            //await GetListApi<InvoiceModelView>($"GetList?ParentId={model.ParentId}");
            //ViewBag.OutlayId = new SelectList(new OutlayService(User.GetSchema()).GetAll(model.ParentId, 0, 1, 20), "Id", "Name", model.OutlayId);
            ViewBag.OutlayId = new SelectList( await GetListApi<OutlayDto>($"GetList?ParentId={model.ParentId}"), "Id", "Name", model.OutlayId);
            //ViewBag.CurrencyId = new SelectList(new CurrencyService(User.GetSchema()).GetAll(model.ParentId, 0, 1, 20), "Id", "Name", model.CurrencyId);
            ViewBag.CurrencyId = new SelectList(await GetListApi<CurrencyDto>() , "Id", "Name", model.CurrencyId);
            //ViewBag.PaymentTypeId = new SelectList(new PaymentTypeService(User.GetSchema()).GetAll(model.ParentId, 0, 1, 20), "Id", "Name", model.PaymentTypeId);
            ViewBag.PaymentTypeId = new SelectList(await GetListApi<PaymentTypeDto>(), "Id", "Name", model.PaymentTypeId);
            //ViewBag.SafeId = new SelectList(new SafeService(User.GetSchema()).GetAll(model.ParentId, 0, 1, 20), "Id", "Name", model.SafeId);
            ViewBag.SafeId = new SelectList( await GetListApi<SafeDto>($"GetList?ParentId={model.ParentId}") , "Id", "Name", model.SafeId);
            //var type = new TransactionTypeService(User.GetSchema()).Get(model.TypeId);
            var type = await GetObApi<TransactionTypeDto>($"GetById?Id={model.TypeId}");
            ViewBag.TransactionsType = type.Name;
            ViewBag.TransactionsType = type.Icon;
        }


        public override async Task<FinancialDto> InitializeData(FinancialDto ob)
        {


            //var setting = new PreferenceService(User.GetSchema());
            var setting = await GetListApi<PreferenceDto>(TypeId: ob.TypeId, TextSearch: "Financial", PageSize: 1000);


            //var SafeId = long.Parse("0" + setting.GetByKey("DefaultSafe", "Financial", ob.TypeId, 0)?.Value);
            var SafeId = long.Parse("0" + setting.FirstOrDefault(e => e.Key == "DefaultSafe")?.Value);
            //var PaymentTypeId = long.Parse("0" + setting.GetByKey("DefaultPaymentType", "Financial", ob.TypeId, 0)?.Value);
            var PaymentTypeId = long.Parse("0" + setting.FirstOrDefault(e => e.Key == "DefaultPaymentType")?.Value);
            //var CurrencyId = long.Parse("0" + setting.GetByKey("DefaultCurrency", "Financial", ob.TypeId, 0)?.Value);
            var CurrencyId = long.Parse("0" + setting.FirstOrDefault(e => e.Key == "DefaultCurrency")?.Value);
            //var OutlayId = long.Parse("0" + setting.GetByKey("DefaultOutlay", "Financial", ob.TypeId, 0)?.Value);
            var OutlayId = long.Parse("0" + setting.FirstOrDefault(e => e.Key == "DefaultOutlay")?.Value);

            long DealerId = 0;
            if (ob.TypeId == 1)
                //DealerId = long.Parse("0" + setting.GetByKey("DefaultSupplier", "Financial", ob.TypeId, 0)?.Value);
                DealerId = long.Parse("0" + setting.FirstOrDefault(e => e.Key == "DefaultClient")?.Value);
            else if (ob.TypeId == 2)
                //DealerId = long.Parse("0" + setting.FirstOrDefault(e => e.Key == "DefaultCustomer")?.Value);
                DealerId = long.Parse("0" + setting.FirstOrDefault(e => e.Key == "DefaultSupplier")?.Value);

            //ViewBag.AutoSave = int.Parse("0" + setting.GetByKey("AutoSave", "Financial", ob.TypeId, 0)?.Value);
            ViewBag.AutoSave = int.Parse("0" + setting.FirstOrDefault(e => e.Key == "AutoSave")?.Value);
            var TypeCode = int.Parse("0" + setting.FirstOrDefault(e => e.Key == "TypeSerial")?.Value);
            //var TypeCode = int.Parse("0" + setting.FirstOrDefault(e => e.Key == "TypeSerial")?.Value);
            ViewBag.TypeSerial = TypeCode;

            if (ob == null)
                ob = new FinancialDto();

            if (ob.Id == 0)
            {
                //ob.CodeNumber = new FinancialService(User.GetSchema()).GetMaxCode(ob.TypeId);
                ob.CodeNumber = long.Parse("0" + await GetValueApi<FinancialDto>($"GetMax?ParentId=0&TypeId={ob.TypeId}")) + 1;
                ob.Code = "" + ob.CodeNumber;
                ob.SafeId = SafeId;
                ob.DealerId = DealerId;
                ob.CurrencyId = long.Parse("0" + CurrencyId);
                //ob.Rate = new CurrencyService(User.GetSchema()).Get(long.Parse("0" + CurrencyId))?.Rate??0;
                ob.Rate = (await GetObApi<CurrencyDto>($"GetById?Id={CurrencyId}")).Rate;
                ob.PaymentTypeId = PaymentTypeId;
                ob.OutlayId = OutlayId;
                ob.Date = DateTime.Now;
                ob.FinancialInvoiceList = new List<FinancialInvoiceDto>();
            }

            //ob.SafeName = new SafeService(User.GetSchema()).Get(ob.SafeId)?.Name;
            ob.SafeName = (await GetObApi<SafeDto>($"GetById?Id={ob.SafeId}"))?.Name;
            ob.DealerName = (await GetObApi<DealerDto>($"GetById?Id={ob.DealerId ?? 0}"))?.Name;
            return ob;
        }

        public async Task<ActionResult> Cancel(long id, string search, long ParentId = 0, long TypeId = 0, int page = 1)
        {
            var response = await ApiMethod(ApiMethodType.Put, $"Cancel?Id={id}");
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<Domain.Shared.Result>(data);

            return Redirect("/Financials/Financial/Index?ParentId=" + ParentId + "&TypeId=" + TypeId + "&page=" + page + "&status=" + (res.StatusCode != null ? ResultStatus.success : ResultStatus.error) + "&MsgError=Success");
        }

        public async Task<ActionResult> Redo(long id, string search, long ParentId = 0, long TypeId = 0, int page = 1)
        {
            var response = await ApiMethod(ApiMethodType.Put, $"Redo?Id={id}");
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<Domain.Shared.Result>(data);

            return Redirect("/Financials/Financial/Index?ParentId=" + ParentId + "&TypeId=" + TypeId + "&page=" + page + "&status=" + (res.StatusCode != null ? ResultStatus.success : ResultStatus.error) + "&MsgError=Success");
        }



        [HttpPost]
        public async Task<ActionResult> AutoSave(FinancialDto ob)
        {
            await base.Save(ob);
            if (ob.Id == 0)
            {

                var key = ob.GetType().GetProperty("Code")?.GetValue(ob, null);
                var searchResp = await ApiMethod(ApiMethodType.Get, $"Search?KeySearch={key}&ParentId={ob.ParentId}&TypeId={ob.TypeId}&Page=1&PageSize=1");
                if (searchResp != null && searchResp.IsSuccessStatusCode)
                {
                    var searchData = await searchResp.Content.ReadAsStringAsync();
                    var searchRes = JsonConvert.DeserializeObject<ResultPagination<FinancialDto>>(searchData);
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
                        url = "/" + "Financial" + "/" + "Financial" + "?ParentId=" + ob.ParentId + "&TypeId=" + ob.TypeId + "&status=" + ResultStatus.success + "&MsgError=Success"
                    });
                }
            }
            return Ok();
        }
    }
}