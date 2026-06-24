namespace OrgSys.Areas.Setting.Controllers
{
    using Application.Commands.Org.Setting.Account.Commands;
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
        private static List<AccountTreeNodeModelView> BuildTree(List<AccountModelView> accounts, long parentId = 0)
        {
            return accounts
                .Where(x => x.ParentId == parentId)
                .Select(x => new AccountTreeNodeModelView
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    Children = BuildTree(accounts, x.Id)
                })
                .ToList();
        }

        public override async Task LoadViewBag(AccountModelView model)
        {
            ViewBag.AccountType = new SelectList(await GetListApi<AccountTypeModelView>(Page: 1, PageSize: 20), "Id", "Name", model.AccountTypeId);
            ViewBag.AccountList = new SelectList(await GetListApi<AccountModelView>( Page: 1, PageSize: 20), "Id", "Name", model.ParentId);
    
        }
        public override async Task LoadViewBagIndex(long ParentId = 0, long TypeId = 0)
        {
            var accounts = await GetListApi<AccountModelView>(Page: 1, PageSize: 100000);
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
    }
}