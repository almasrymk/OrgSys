namespace OrgSys.Areas.Setting.Controllers
{
    using Accounting.Application.Accounts.Commands;
    using AutoMapper;
    using Application.DTOs;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Extensions.Configuration;
    using OrgSys.Controllers;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Area("Setting")]
    public class AccountController(IConfiguration configuration, IMapper mapper) : MainController<AccountDto, CreateAccountCommand, UpdateAccountCommand>(configuration, mapper)
    {
        private static List<AccountTreeNodeDto> BuildTree(List<AccountDto> accounts, long parentId = 0)
        {
            return accounts
                .Where(x => x.ParentId == parentId)
                .Select(x => new AccountTreeNodeDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    Children = BuildTree(accounts, x.Id)
                })
                .ToList();
        }

        public override async Task LoadViewBag(AccountDto model)
        {
            ViewBag.AccountType = new SelectList(await GetListApi<AccountTypeDto>(Page: 1, PageSize: 20), "Id", "Name", model.AccountTypeId);
            ViewBag.AccountList = new SelectList(await GetListApi<AccountDto>( Page: 1, PageSize: 20), "Id", "Name", model.ParentId);
    
        }
        public override async Task LoadViewBagIndex(long ParentId = 0, long TypeId = 0)
        {
            var accounts = await GetListApi<AccountDto>(Page: 1, PageSize: 100000);
            ViewBag.AccountTree = BuildTree(accounts);
        }

        public override async Task<AccountDto> InitializeData(AccountDto ob)
        {
            ViewBag.AccountList = new SelectList(await GetListApi<AccountDto>(Page: 1, PageSize: 20), "Id", "Name", ob.ParentId);
            if (ob == null)
                ob = new AccountDto();
            if (ob.Id == 0)
            {
                ob.CodeNumber = long.Parse("0" + await GetValueApi<AccountDto>($"GetMax")) + 1;
                ob.Code = "" + ob.CodeNumber;
            }
            ob.ParentName = (await GetObApi<AccountDto>($"GetById?Id={ob.ParentId}"))?.Name;
            return ob;
        }

        public async Task<JsonResult> GetList(string txtSearch = "", int page = 1, int pageSize = 20)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();

            var itemsList = await GetListApi<AccountDto>(TextSearch:txtSearch , Page: page, PageSize: pageSize);
            var list = itemsList.Distinct().OrderBy(_ => _.Name)
                .Select(_ => new
                {
                    _.Id,
                    _.Name
                })
                .ToList();
            return Json(list);
        }

        public async Task<JsonResult> SearchAccounts(string phrase = "")
        {          
            var setting = await GetListApi<PreferenceDto>(Page: 1, PageSize: 1000);
            if (phrase == null)
                phrase = "";
            phrase = phrase.Trim().ToLower();

            var CodeElectronicScale = setting.FirstOrDefault(e => e.Key == "CodeElectronicScale" && e.Reference == "Journal")?.Value;
            int.TryParse(setting.FirstOrDefault(e => e.Key == "LengthElectronicScale" && e.Reference == "Journal")?.Value, out int LengthElectronicScale);
            if (phrase.Length >= LengthElectronicScale && "" + CodeElectronicScale != "" && "" + CodeElectronicScale != "0" && "" + LengthElectronicScale != "" && "" + LengthElectronicScale != "0")
            {
                if (phrase.StartsWith(CodeElectronicScale))
                {
                    var code = phrase.Substring(0, LengthElectronicScale);                   
                    phrase = code;
                }
            }

            var accountsList = await GetListApi<AccountDto>(TextSearch: phrase, Page: 1, PageSize: 20);
            var list = accountsList.Distinct().OrderBy(_ => _.Name)
                .Select(_ => new
                {
                    _.Id,
                    _.Name,
                    _.Debit,
                    _.Credit,                   
                    Code = phrase == _.Code ? Domain.Resource.Title_Designer.Code + " " + _.Code : "",
                    ParentName = "" + phrase != "" && _.ParentName.ToLower().Contains("" + phrase) ? _.ParentName : ""
                })
                .ToList();
            return Json(list);
        }

        public async Task<JsonResult> checkStock(int id)
        {
            var product = (await GetObApi<AccountDto>($"GetById?Id={id}"));
            var data = new
            {
                id = product.Id,
                code = product.Code,
                CodeNumber = product.CodeNumber,
                name = product.Name,
                Debit = product.Debit,
                Credit = product.Credit,                
            };
            return Json(data);
        }
    }
}