namespace OrgSys.Areas.Setting.Controllers
{
    using Parties.Application.Dealers.Commands;
    using AutoMapper;
    using System.Net.Http;
    using OrgSys.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using OrgSys.Controllers;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Area("Setting")]
    public class DealerController(IConfiguration configuration, IMapper mapper) : MainController<DealerDto, CreateDealerCommand, UpdateDealerCommand>(configuration, mapper)
    {

        public override async Task LoadViewBag(DealerDto model)
        {
            ViewBag.DealersGroupList = new SelectList(await GetListApi<DealerGroupDto>(TypeId: model.TypeId, Page: 1, PageSize: 20), "Id", "Name", model.DealerGroupId);
            ViewBag.AccountList = new SelectList(await GetListApi<AccountDto>(TypeId: model.TypeId, Page: 1, PageSize: 20), "Id", "Name", model.AccountId);
        }

        public override async Task<DealerDto> InitializeData(DealerDto ob)
        {
            ViewBag.DealersGroupList = new SelectList(await GetListApi<DealerGroupDto>(TypeId: ob.TypeId , Page: 1, PageSize: 20), "Id", "Name", ob.DealerGroupId);
            ViewBag.AccountList = new SelectList(await GetListApi<AccountDto>(TypeId: ob.TypeId, Page: 1, PageSize: 20), "Id", "Name", ob.AccountId);

            if (ob == null)
                ob = new DealerDto();
            if (ob.Id == 0)
            {
                ob.CodeNumber = long.Parse("0" + await GetValueApi<DealerDto>($"GetMax?TypeId={ob.TypeId}")) + 1;
                ob.Code = "" + ob.CodeNumber;
            }
            ob.DealerGroupName = (await GetObApi<DealerGroupDto>($"GetById?Id={ob.DealerGroupId ?? 0}"))?.Name;
            ob.AccountName = (await GetObApi<AccountDto>($"GetById?Id={ob.AccountId ?? 0}"))?.Name;

            if (ob.Id > 0 && ob.AccountId is > 0)
            {
                var balanceResponse = await ApiMethod(HttpMethod.Get, $"Balance?Id={ob.Id}");
                if (balanceResponse.IsSuccessStatusCode)
                {
                    var balanceData = await balanceResponse.Content.ReadAsStringAsync();
                    var balance = JsonConvert.DeserializeObject<Result<decimal>>(balanceData)?.Response;
                    if (ob.TypeId == (long)DealerType.Supplier)
                        // GetDealerBalanceQuery returns Debit - Credit; a payable is conventionally shown as a credit balance.
                        ViewBag.SupplierBalance = balance is null ? null : -balance;
                    else
                        ViewBag.CustomerBalance = balance;
                }
            }

            return ob;
        }

        public async Task<JsonResult> GetList(string txtSearch = "", long TypeId = 0, int page = 1, int pageSize = 20)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();
            long TypeDealerId = TypeId == 1 || TypeId == 3 ? 1 : 2;

            var itemsList = await GetListApi<DealerDto>(TypeId: TypeDealerId , TextSearch: txtSearch, Page: page, PageSize: pageSize);
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