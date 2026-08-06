using Application.Commands.Org.Transactions.Inventory.Commands;
using AutoMapper;
using Domain.Enums;
using Domain.Shared;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OrgSys.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgSys.Areas.Inventory.Controllers
{
    [Area("Transactions")]
    public class InventoryController(IConfiguration configuration, IMapper mapper) : MainController<InventoryDto, CreateInventoryCommand , UpdateInventoryCommand>(configuration , mapper)
    {       
        public override async Task<InventoryDto> InitializeData(InventoryDto ob)
        {
            //var setting =  new PreferenceService(User.GetSchema());
            var setting = await GetListApi<PreferenceDto>(TypeId: ob.TypeId, TextSearch: "Inventory");
            //var StockId = long.Parse("0" + setting.GetByKey("DefaultStock", "Inventory", ob.TypeId, 0)?.Value);           
            var StockId = long.Parse("0" + setting.FirstOrDefault(e => e.Key == "DefaultStock" && e.TypeId == ob.TypeId && e.Reference == "Inventory")?.Value);           
            //ViewBag.AutoSave = int.Parse("0" + setting.GetByKey("AutoSave", "Inventory", ob.TypeId, 0)?.Value);
            ViewBag.AutoSave = long.Parse("0" + setting.FirstOrDefault(e => e.Key == "AutoSave" && e.TypeId == ob.TypeId && e.Reference == "Inventory")?.Value);
            //var TypeCode = int.Parse("0" + setting.GetByKey("TypeSerial", "Inventory", ob.TypeId, 0)?.Value);
            var TypeCode = long.Parse("0" + setting.FirstOrDefault(e => e.Key == "TypeSerial" && e.TypeId == ob.TypeId && e.Reference == "Inventory")?.Value);
            ViewBag.TypeSerial = TypeCode;

            if (ob == null)
                ob = new InventoryDto();

            if (ob.Id == 0)
            {
                //ob.CodeNumber = new InventoryService(User.GetSchema()).GetMaxCode(ob.TypeId);
                ob.CodeNumber = long.Parse("0" + await GetValueApi<InventoryDto>($"GetMax?ParentId=0&TypeId={ob.TypeId}")) + 1;
                ob.Code = "" + ob.CodeNumber;
                ob.StockId = StockId;
                ob.Date = DateTime.Now;
                ob.InventoryProductList = new List<InventoryProductDto>();
              }
            //ob.StockName = new StockService(User.GetSchema()).Get(ob.StockId??0).Name;
            ob.StockName = (await GetObApi<StockDto>($"GetById?Id={ob.StockId}"))?.Name;
            return ob;
        }

        public override Task<InventoryDto> FixData(InventoryDto ob)
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

        public async Task<ActionResult> Cancel(long id, string search, long ParentId = 0, long TypeId = 0, int page = 1)
        {
            var response = await ApiMethod(ApiMethodType.Put, $"Cancel?Id={id}");
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<Result>(data);

            return Redirect("/Transactions/Inventory/Index?ParentId=" + ParentId + "&TypeId=" + TypeId + "&page=" + page + "&status=" +
                (res?.StatusCode != null ? ResultStatus.success : ResultStatus.error) + "&MsgError=Success");
        }

        public async Task<ActionResult> Redo(long id, string search, long ParentId = 0, long TypeId = 0, int page = 1)
        {
            var response = await ApiMethod(ApiMethodType.Put, $"Redo?Id={id}");
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<Result>(data);

            return Redirect("/Transactions/Inventory/Index?ParentId=" + ParentId + "&TypeId=" + TypeId + "&page=" + page + "&status=" +
                (res?.StatusCode != null ? ResultStatus.success : ResultStatus.error) + "&MsgError=Success");
        }


        [HttpPost]
        public async Task<ActionResult> AutoSave(InventoryDto ob)
        {
            await base.Save(ob);
            if (ob.Id == 0)
            {

                var key = ob.GetType().GetProperty("Code")?.GetValue(ob, null);
                var searchResp = await ApiMethod(ApiMethodType.Get, $"Search?KeySearch={key}&ParentId={ob.ParentId}&TypeId={ob.TypeId}&Page=1&PageSize=1");
                if (searchResp != null && searchResp.IsSuccessStatusCode)
                {
                    var searchData = await searchResp.Content.ReadAsStringAsync();
                    var searchRes = JsonConvert.DeserializeObject<ResultPagination<InventoryDto>>(searchData);
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
                        url = "/" + "Inventory" + "/" + "Inventory" + "?ParentId=" + ob.ParentId + "&TypeId=" + ob.TypeId + "&status=" + ResultStatus.success + "&MsgError=Success"
                    });
                }
            }
            return Ok();
        }

    }
}
