using Application.Commands.Org.Setting.Product.Commands;
using Application.Commands.Org.Transactions.Transaction.Commands;
using Application.Commands.Org.Transactions.TransactionType.Commands;
using AutoMapper;
using Domain.Enums;
using Domain.Shared;
using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OrgSys.Controllers;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace OrgSys.Areas.Transaction.Controllers
{
    [Area("Transactions")]
    public class TransactionController(IConfiguration configuration, IMapper mapper) : MainController<TransactionModelView, CreateTransactionCommand, UpdateTransactionCommand>(configuration, mapper)
    {
        public override async Task LoadViewBagIndex(long ParentId = 0, long TypeId = 0)   
        {
            //var type = new TransactionTypeService(User.GetSchema()).Get(TypeId);
            var type = await GetObApi<TransactionTypeModelView>($"GetById?Id={TypeId}");
            ViewBag.TransactionsType = type.MaskText;            
            ViewBag.TransactionsIcon = type.Icon;
        }

        public override async Task LoadViewBag(TransactionModelView model)
        {

            //var type = new TransactionTypeService(User.GetSchema()).Get(model.TypeId);
            var type = await GetObApi<TransactionTypeModelView>($"GetById?Id={model.TypeId}");
            ViewBag.TransactionsType = type.Name;
            ViewBag.TransactionsType = type.Icon;
        }

        public override Task<TransactionModelView> FixData(TransactionModelView ob)
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

        public override async Task<TransactionModelView> InitializeData(TransactionModelView ob)
        {
            
            //var setting = new PreferenceService(User.GetSchema());
            var setting = await GetListApi<PreferenceModelView>(TypeId: ob.TypeId, TextSearch: "Transaction");
            var StockId = long.Parse("0" + setting.FirstOrDefault(e => e.Key == "DefaultStock" && e.Reference == "Transaction" && e.TypeId == ob.TypeId)?.Value);

            long DealerId = 0;
            if (ob.TypeId == 1)
                DealerId = long.Parse("0" + setting.FirstOrDefault(e => e.Key == "DefaultSupplier" && e.Reference == "Transaction" && e.TypeId == ob.TypeId)?.Value);
            else if (ob.TypeId == 2)
                DealerId = long.Parse("0" + setting.FirstOrDefault(e => e.Key == "DefaultCustomer" && e.Reference == "Transaction" && e.TypeId == ob.TypeId)?.Value);

            ViewBag.NumberLine = int.Parse("0" + setting.FirstOrDefault(e => e.Key == "NumberLine" && e.Reference == "Transaction" && e.TypeId == ob.TypeId)?.Value);
            ViewBag.OrderTabe = int.Parse("0" + setting.FirstOrDefault(e => e.Key == "OrderTabe" && e.Reference == "Transaction" && e.TypeId == ob.TypeId)?.Value);
            ViewBag.AutoSave = int.Parse("0" + setting.FirstOrDefault(e => e.Key == "AutoSave" && e.Reference == "Transaction" && e.TypeId == ob.TypeId)?.Value);
            var TypeCode = int.Parse("0" + setting.FirstOrDefault(e => e.Key == "TypeSerial" && e.Reference == "Transaction" && e.TypeId == ob.TypeId)?.Value);
            ViewBag.TypeSerial = TypeCode;
            ViewBag.AllowRepeated = int.Parse("0" + setting.FirstOrDefault(e => e.Key == "AllowRepeated" && e.Reference == "Transaction" && e.TypeId == ob.TypeId)?.Value);

            if (ob == null)
                ob = new TransactionModelView();

            if (ob.Id == 0)
            {

                ob.CodeNumber = long.Parse("0" + await GetValueApi<TransactionModelView>($"GetMax?ParentId=0&TypeId={ob.TypeId}")) + 1;      
                ob.Code = "" + ob.CodeNumber;
                ob.StockId = StockId;
                ob.DealerId = DealerId;
                ob.Date = DateTime.Now;
                ob.TransactionProductList = new List<TransactionProductModelView>();
            }

            ob.StockName = (await GetObApi<StockModelView>($"GetById?Id={ob.StockId}"))?.Name;
            ob.ToStockName = (await GetObApi<StockModelView>($"GetById?Id={ob.StockId}"))?.Name;
            ob.DealerName = (await GetObApi<DealerModelView>($"GetById?Id={ob.DealerId?? 0 }"))?.Name;
            return ob;
        }



        [HttpPost]
        public async Task<ActionResult> AutoSave(TransactionModelView ob)
        {
            await base.Save(ob);
            if (ob.Id == 0)
            {

                var key = ob.GetType().GetProperty("Code")?.GetValue(ob, null);
                var searchResp = await ApiMethod(ApiMethodType.Get, $"Search?KeySearch={key}&ParentId={ob.ParentId}&TypeId={ob.TypeId}&Page=1&PageSize=1");
                if (searchResp != null && searchResp.IsSuccessStatusCode)
                {
                    var searchData = await searchResp.Content.ReadAsStringAsync();
                    var searchRes = JsonConvert.DeserializeObject<ResultPagination<TransactionModelView>>(searchData);
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
                        url = "/" + "Transactions" + "/" + "Transaction" + "?ParentId=" + ob.ParentId + "&TypeId=" + ob.TypeId + "&status=" + ResultStatus.success + "&MsgError=Success"
                    });
                }
            }
            return Ok();
        }
    }
}