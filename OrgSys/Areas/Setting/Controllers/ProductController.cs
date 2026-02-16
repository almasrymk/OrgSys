namespace OrgSys.Areas.Setting.Controllers
{
    using Application.Commands.Org.Setting.Product.Commands;
    using AutoMapper;
    using Entity.ModelView;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Extensions.Configuration;
    using OrgSys.Controllers;
    using Service;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Area("Setting")]
    public class ProductController(IConfiguration configuration, IMapper mapper) : MainController<ProductModelView, CreateProductCommand, UpdateProductCommand>(configuration, mapper)
    {
        public override async Task LoadViewBag(ProductModelView model)
        {
            ViewBag.UnitList = new SelectList(await GetListApi<UnitModelView>(Page: 1, PageSize: 20), "Id", "Name");
        }

        public override async Task<ProductModelView> InitializeData(ProductModelView ob)
        {
            if (ob.ProductUnitList == null)
                ob.ProductUnitList = new List<ProductUnitModelView>();
            if (ob.Id == 0)
            {
                ob.CodeNumber = long.Parse("0" + await GetValueApi<ProductModelView>($"GetMax?TypeId={ob.TypeId}")) + 1;
                ob.Code = "" + ob.CodeNumber;
            }
            ob.ClassificationName = (await GetObApi<ClassificationModelView>($"GetById?Id={ob.ClassificationId}"))?.Name;
            ob.DealerName = (await GetObApi<DealerModelView>($"GetById?Id={ob.DealerId ?? 1}"))?.Name;
            return ob;
        }

        public async Task<JsonResult> GetUnitNameListByProductId(int ProductId)
        {
            var ProductUnitList = await GetListApi<ProductUnitModelView>($"GetByProductId?ProductId={ProductId}&Page=1&PageSize=20");
            if (ProductUnitList == null)
                ProductUnitList = new List<ProductUnitModelView>();
            var UnitNameList = new SelectList(ProductUnitList.Select(e => e.UnitName));
            return Json(new { success = true, UnitNameList });
        }

        public async Task<ActionResult> SearchProducts(string txt = "", int page = 1, int Type = 1, int index = 0)
        {
            ViewBag.index = index;
            var list = await GetListApi<ProductModelView>(TextSearch:txt , Page: page , PageSize:20);
            return Type != 1 ? (ActionResult)PartialView("SearchProductsList", list) : View("SearchProducts", list);
        }

        public async Task<JsonResult> SearchItems(string phrase = "", int TypeInv = 1)
        {
            decimal Quantity = 1;
            var setting = await GetListApi<PreferenceModelView>(TypeId: TypeInv, Page: 1, PageSize: 1000);
            if (phrase == null)
                phrase = "";
            phrase = phrase.Trim().ToLower();

            var CodeElectronicScale = setting.FirstOrDefault(e => e.Key == "CodeElectronicScale" && e.Reference == "Invoice")?.Value;
            var LengthElectronicScale = int.Parse(setting.FirstOrDefault(e => e.Key == "LengthElectronicScale" && e.Reference == "Invoice")?.Value);
            var LengthQtyElectronicScale = int.Parse(setting.FirstOrDefault(e => e.Key == "LengthQtyElectronicScale" && e.Reference == "Invoice")?.Value);

            if (phrase.Length >= LengthElectronicScale && "" + CodeElectronicScale != "" && "" + CodeElectronicScale != "0" && "" + LengthElectronicScale != "" && "" + LengthElectronicScale != "0")
            {
                if (phrase.StartsWith(CodeElectronicScale))
                {
                    var code = phrase.Substring(0, LengthElectronicScale);
                    var qty = phrase.Substring(LengthElectronicScale);
                    Quantity = decimal.Parse("0" + qty) / 1000;
                    phrase = code;
                }
            }

            var itemsList = await GetListApi<ProductModelView>(TextSearch: phrase, Page: 1, PageSize: 20);
            var list = itemsList.Distinct().OrderBy(_ => _.Name)
                .Select(_ => new
                {
                    _.Id,
                    _.Name,
                    Barcode = phrase == _.Barcode ? Utility.Resource.Title_Designer.Barcode + " " + _.Barcode : "",
                    _.Price,
                    _.Cost,
                    quantity = Quantity,
                    Code = phrase == _.Code ? Utility.Resource.Title_Designer.Code + " " + _.Code : "",
                    ClassificationName = "" + phrase != "" && _.ClassificationName.ToLower().Contains("" + phrase) ? _.ClassificationName : ""
                })
                .ToList();
            return Json(list);
        }

        public async Task<JsonResult> SearchItemName(string txtSearch = "", int TypeInv = 1)
        {
            decimal Quantity = 1;
            var setting = await GetListApi<PreferenceModelView>(TypeId: TypeInv, Page: 1, PageSize: 1000);
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();

            var CodeElectronicScale = setting.FirstOrDefault(e => e.Key == "CodeElectronicScale" && e.Reference == "Invoice")?.Value;
            var LengthElectronicScale = int.Parse(setting.FirstOrDefault(e => e.Key == "LengthElectronicScale" && e.Reference == "Invoice")?.Value);
            var LengthQtyElectronicScale = int.Parse(setting.FirstOrDefault(e => e.Key == "LengthQtyElectronicScale" && e.Reference == "Invoice")?.Value);

            if (txtSearch.Length >= LengthElectronicScale && "" + CodeElectronicScale != "" && "" + CodeElectronicScale != "0" && "" + LengthElectronicScale != "" && "" + LengthElectronicScale != "0")
            {
                if (txtSearch.StartsWith(CodeElectronicScale))
                {
                    var code = txtSearch.Substring(0, LengthElectronicScale);
                    var qty = txtSearch.Substring(LengthElectronicScale);
                    Quantity = decimal.Parse("0" + qty) / 1000;
                    txtSearch = code;
                }
            }

            var item = new ProductService(User.GetSchema()).Get(txtSearch);
            if (item == null)
                item = new ProductModelView();

            item.Quantity = Quantity;
            return Json(item);
        }

        public async Task<JsonResult> checkStock(int id)
        {
            var product = (await GetObApi<ProductModelView>($"GetById?Id={id}"));
            var data = new
            {
                id = product.Id,
                name = product.Name,
                price = product.Price,
                cost = product.Cost,
                selectunitid = product.ProductUnitList.FirstOrDefault(e => e.DefaultUnit).UnitId,
                selectunitName = product.ProductUnitList.FirstOrDefault(e => e.DefaultUnit).UnitName,
                unitlist = product.ProductUnitList,// new UnitService(User.GetSchema()).GetAllByProductId(id)
            };
            return Json(data);
        }

        public async Task<JsonResult> LoadProductsByStock(long StockId, DateTime date)
        {
            //var products = new ProductService(User.GetSchema()).GetAllByBalance(StockId, date);
            var products = await GetListApi<ProductModelView>($"GetAllByBalance?StockId={StockId}&date={date}");
            var data = products.Select(e => new
            {
                id = e.Id,
                name = e.Name,
                price = e.Price,
                selectunitid = e.ProductUnitList.FirstOrDefault(e => e.DefaultUnit).UnitId,
                selectunitName = e.ProductUnitList.FirstOrDefault(e => e.DefaultUnit).UnitName,
                balance = e.Balance
            }).ToList();
            return Json(data);
        }
    }
}