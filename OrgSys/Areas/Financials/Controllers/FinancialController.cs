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
using System.Net;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Net.Http;

namespace OrgSys.Areas.Financial.Controllers
{
    [Area("Financials")]
    public class FinancialController(IConfiguration configuration, IMapper mapper) :
        MainController<FinancialDto, CreateFinancialCommand , UpdateFinancialCommand>(configuration , mapper)
    {
        public override async Task LoadViewBagIndex(long ParentId = 0, long TypeId = 0)
        {
            ViewBag.FinancialsType = "Financial Transactions";
            ViewBag.FinancialsIcon = "iconsminds-coins";
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

            using var client = CreateClient();
            var accountsResponse = await client.GetAsync($"{Configuration["ApiUrl"]}/Financial/Accounts");
            accountsResponse.EnsureSuccessStatusCode();
            var accountsResult = JsonConvert.DeserializeObject<ResultCollection<FinancialAccountDto>>(
                await accountsResponse.Content.ReadAsStringAsync());
            ViewBag.FinancialAccountId = new SelectList(accountsResult?.Response ?? [], "Id", "Name", model.FinancialAccountId);

            ViewBag.FinancialTypeId = new SelectList(await GetListApi<FinancialTypeDto>(), "Id", "Name", model.FinancialTypeId);

            ViewBag.CounterAccountId = new SelectList(await GetListApi<AccountDto>(), "Id", "Name", model.CounterAccountId);
            ViewBag.CurrencyId = new SelectList(await GetListApi<CurrencyDto>() , "Id", "Name", model.CurrencyId);
        }


        public override async Task<FinancialDto> InitializeData(FinancialDto ob)
        {

            if (ob.Id == 0)
            {
                ob.Date = DateTime.Today;
                ob.Rate = 1;
                ob.Direction = FinancialTransactionDirection.In;
                ob.FinancialTypeId = ob.TypeId > 0 ? ob.TypeId : 1;
                ob.ReferenceType = FinancialReferenceType.Other;
            }

            return ob;
        }

        [HttpPost, ValidateAntiForgeryToken]
        public override async Task<ActionResult> Save(FinancialDto ob)
        {
            if (ob.FinancialAccountId is null or <= 0)
                ModelState.AddModelError(nameof(ob.FinancialAccountId), "Financial account is required.");
            if (ob.FinancialTypeId is null or <= 0)
                ModelState.AddModelError(nameof(ob.FinancialTypeId), "Financial type is required.");
            if (ob.Amount <= 0)
                ModelState.AddModelError(nameof(ob.Amount), "Amount must be greater than zero.");
            if (ob.CounterAccountId <= 0)
                ModelState.AddModelError(nameof(ob.CounterAccountId), "Counter GL account is required.");

            if (ModelState.IsValid)
            {
                var command = new PostFinancialTransactionDto
                {
                    FinancialAccountId = ob.FinancialAccountId!.Value,
                    FinancialTypeId = ob.FinancialTypeId!.Value,
                    Direction = ob.Direction ?? FinancialTransactionDirection.In,
                    Amount = ob.Amount,
                    CurrencyId = ob.CurrencyId,
                    ExchangeRate = ob.Rate,
                    TransactionDate = ob.Date,
                    ReferenceType = ob.ReferenceType,
                    ReferenceId = ob.ReferenceId,
                    CounterAccountId = ob.CounterAccountId,
                    Description = ob.Notes,
                    CreateUserId = User.GetUserId(),
                    BranchId = ob.BranchId,
                    ShiftId = ob.ShiftId
                };
                using var client = CreateClient();
                var response = await client.PostAsJsonAsync($"{Configuration["ApiUrl"]}/Financial/Transactions/Post", command);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index), new { TypeId = command.FinancialTypeId, status = ResultStatus.success, MsgError = "Success" });
                ModelState.AddModelError(string.Empty, await response.Content.ReadAsStringAsync());
            }
            await LoadViewBag(ob);
            return View(ob);
        }

        public async Task<ActionResult> Cancel(long id, string search, long ParentId = 0, long TypeId = 0, int page = 1)
        {
            var response = await ApiMethod(ApiMethodType.Put, $"Cancel?Id={id}");
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<Domain.Shared.Result>(data);

            return Redirect("/Financials/Financial/Index?ParentId=" + ParentId + "&TypeId=" + TypeId + "&page=" + page + "&status=" + (res.StatusCode == HttpStatusCode.OK ? ResultStatus.success : ResultStatus.error) + "&MsgError=Success");
        }

        public async Task<ActionResult> Redo(long id, string search, long ParentId = 0, long TypeId = 0, int page = 1)
        {
            var response = await ApiMethod(ApiMethodType.Put, $"Redo?Id={id}");
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<Domain.Shared.Result>(data);

            return Redirect("/Financials/Financial/Index?ParentId=" + ParentId + "&TypeId=" + TypeId + "&page=" + page + "&status=" + (res.StatusCode == HttpStatusCode.OK ? ResultStatus.success : ResultStatus.error) + "&MsgError=Success");
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
