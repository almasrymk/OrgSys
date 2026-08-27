#nullable enable annotations
using Application.Commands.Org.Financials.FinancialAccount.Commands;
using Application.DTOs;
using AutoMapper;
using Domain.Enums;
using Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OrgSys.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgSys.Areas.Setting.Controllers;

/// <summary>Cash Boxes and Bank Accounts screens — both are a <c>FinancialAccount</c> (CashBox/Bank
/// discriminated by <see cref="FinancialAccountType"/>) filtered by the generic <c>TypeId</c> filter
/// dimension (same convention <c>Financials/Financial</c> uses for its own sub-types), mirroring how
/// the Dealer controller multiplexes Client/Supplier under one entity. Structured like
/// <see cref="AccountController"/>: inherits <see cref="MainController{TDto,TCreate,TUpdate}"/> for
/// Index/Save/Delete/DeleteList and only overrides the extension points (LoadViewBag,
/// LoadViewBagIndex, InitializeData, Save) it genuinely needs. The API side
/// (API/Controllers/Org/Financials/FinancialAccountController.cs) mirrors Setting/Account's
/// GetById/GetList/Search/Create/Update/Delete/DeleteList shape; Create/Update both delegate to the
/// existing <c>SaveFinancialAccountCommand</c> handler so the CashBox/BankAccount dual-write stays in
/// one place.</summary>
[Area("Setting")]
public sealed class FinancialAccountController(IConfiguration configuration, IMapper mapper)
    : MainController<FinancialAccountDto, CreateFinancialAccountCommand, UpdateFinancialAccountCommand>(configuration, mapper)
{
    public override async Task LoadViewBag(FinancialAccountDto model)
    {
        ViewBag.CurrencyList = new SelectList(await GetListApi<CurrencyDto>(PageSize: 500), "Id", "Name", model.CurrencyId);
        ViewBag.BranchList = new SelectList(await GetListApi<BranchDto>(PageSize: 500), "Id", "Name", model.BranchId);
        ViewBag.UserList = new SelectList(await GetListApi<UserDto>(PageSize: 500), "Id", "Name", model.KeeperUserId);
        ViewBag.BankList = new SelectList(await GetListApi<BankDto>(PageSize: 500), "Id", "Name", model.BankId);
        ViewBag.BankBranchList = await GetListApi<BankBranchDto>(PageSize: 500);
    }

    public override async Task LoadViewBagIndex(long ParentId = 0, long TypeId = 0)
    {
        ViewBag.AccountType = TypeId == (long)FinancialAccountType.Bank ? FinancialAccountType.Bank : FinancialAccountType.CashBox;
    }

    public override async Task<FinancialAccountDto> InitializeData(FinancialAccountDto ob)
    {
        if (ob.Id == 0)
        {
            // Cash Box and Bank Account get their own code sequence (GetMax is filtered by TypeId server-side).
            ob.CodeNumber = long.Parse("0" + await GetValueApi<FinancialAccountDto>($"GetMax?TypeId={ob.TypeId}")) + 1;
            ob.Code = "" + ob.CodeNumber;
        }
        else
        {
            var response = await ApiMethod(ApiMethodType.Get, $"{ob.Id}/Balance");
            if (response.IsSuccessStatusCode)
                ViewBag.Balance = JsonConvert.DeserializeObject<Result<decimal>>(
                    await response.Content.ReadAsStringAsync())?.Response;
        }
        return ob;
    }

    // The base Save(GET) always overwrites ob.TypeId with the querystring TypeId (0 on the "Save?id=" edit
    // links List.cshtml renders, since TypeId there is only carried on FinancialAccountType, not the generic
    // BaseModel.TypeId) and never touches FinancialAccountType at all — fine for a brand-new record filled in
    // from the Add link's TypeId, but it leaves ViewBag.TypeId wrong for the Back link on edit, and leaves a
    // NEW record's FinancialAccountType at its zero-value default instead of the type being added. Both are
    // fixed up here once, after the base call (keeping model.TypeId in sync too, since Save.cshtml round-trips
    // it as a hidden field on POST — the base Save(POST) success redirect builds its TypeId query param from
    // ob.TypeId, not FinancialAccountType), rather than duplicating the whole GET action.
    public override async Task<ActionResult> Save(long id = 0, long ParentId = 0, long TypeId = 0, ResultStatus status = ResultStatus.nothing, string MsgError = "")
    {
        var result = await base.Save(id, ParentId, TypeId, status, MsgError);
        if (result is ViewResult { Model: FinancialAccountDto model })
        {
            if (id == 0)
                model.FinancialAccountType = TypeId == (long)FinancialAccountType.Bank ? FinancialAccountType.Bank : FinancialAccountType.CashBox;
            model.TypeId = (long)model.FinancialAccountType;
            ViewBag.TypeId = model.TypeId;
        }
        return result;
    }

    // Used by the account-picker autocomplete on FinancialTransfer/Save.cshtml (/Setting/FinancialAccount/GetList).
    public async Task<JsonResult> GetList(string? txtSearch = null, FinancialAccountType? accountType = null)
    {
        var rows = await GetListApi<FinancialAccountDto>(TypeId: accountType.HasValue ? (long)accountType.Value : 0, TextSearch: txtSearch ?? "", PageSize: 500);
        if (!string.IsNullOrWhiteSpace(txtSearch))
            rows = rows.Where(e => e.Name.Contains(txtSearch, System.StringComparison.OrdinalIgnoreCase)
                || (e.Code ?? "").Contains(txtSearch, System.StringComparison.OrdinalIgnoreCase)).ToList();
        return Json(rows.Select(e => new { e.Id, e.Name, type = e.FinancialAccountType.ToString(), e.CurrencyId }));
    }
}
