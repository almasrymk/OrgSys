namespace OrgSys.Areas.Setting.Controllers
{
    using Application.Commands.Org.Setting.Product.Commands;
    using AutoMapper;
    using Application.DTOs;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Extensions.Configuration;
    using OrgSys.Controllers;
    
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Area("Setting")]
    public class ProductController(IConfiguration configuration, IMapper mapper) : MainController<ProductDto, CreateProductCommand, UpdateProductCommand>(configuration, mapper)
    {
        public override async Task LoadViewBag(ProductDto model)
        {
            ViewBag.UnitList = new SelectList(await GetListApi<UnitDto>(Page: 1, PageSize: 20), "Id", "Name");
        }

        public override async Task<ProductDto> InitializeData(ProductDto ob)
        {
            if (ob.ProductUnits == null)
                ob.ProductUnits = new List<ProductUnitDto>();
            if (ob.Id == 0)
            {
                ob.CodeNumber = long.Parse("0" + await GetValueApi<ProductDto>($"GetMax?TypeId={ob.TypeId}")) + 1;
                ob.Code = "" + ob.CodeNumber;
            }
            ob.ClassificationName = (await GetObApi<ClassificationDto>($"GetById?Id={ob.ClassificationId}"))?.Name;
            ob.DealerName = (await GetObApi<DealerDto>($"GetById?Id={ob.DealerId ?? 1}"))?.Name;
            return ob;
        }

        public async Task<JsonResult> GetUnitNameListByProductId(int ProductId)
        {
            var ProductUnitList = await GetListApi<ProductUnitDto>($"GetByProductId?ProductId={ProductId}&Page=1&PageSize=20");
            if (ProductUnitList == null)
                ProductUnitList = new List<ProductUnitDto>();
            var UnitNameList = new SelectList(ProductUnitList.Select(e => e.UnitName));
            return Json(new { success = true, UnitNameList });
        }

        public async Task<ActionResult> SearchProducts(string txt = "", int page = 1, int Type = 1, int index = 0)
        {
            ViewBag.index = index;
            var list = await GetListApi<ProductDto>(TextSearch:txt , Page: page , PageSize:20);
            return Type != 1 ? (ActionResult)PartialView("SearchProductsList", list) : View("SearchProducts", list);
        }

        public async Task<JsonResult> SearchItems(string phrase = "", int TypeInv = 1)
        {
            decimal Quantity = 1;
            var setting = await GetListApi<PreferenceDto>(TypeId: TypeInv, Page: 1, PageSize: 1000);
            if (phrase == null)
                phrase = "";
            phrase = phrase.Trim().ToLower();

            var CodeElectronicScale = setting.FirstOrDefault(e => e.Key == "CodeElectronicScale" && e.Reference == "Invoice")?.Value;
            int.TryParse(setting.FirstOrDefault(e => e.Key == "LengthElectronicScale" && e.Reference == "Invoice")?.Value, out int LengthElectronicScale); 
            int.TryParse(setting.FirstOrDefault(e => e.Key == "LengthQtyElectronicScale" && e.Reference == "Invoice")?.Value, out int LengthQtyElectronicScale);
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

            var itemsList = await GetListApi<ProductDto>(TextSearch: phrase, Page: 1, PageSize: 20);
            var list = itemsList.Distinct().OrderBy(_ => _.Name)
                .Select(_ => new
                {
                    _.Id,
                    _.Name,
                    Barcode = phrase == _.Barcode ? Domain.Resource.Title_Designer.Barcode + " " + _.Barcode : "",
                    _.Price,
                    _.Cost,
                    quantity = Quantity,
                    Code = phrase == _.Code ? Domain.Resource.Title_Designer.Code + " " + _.Code : "",
                    ClassificationName = "" + phrase != "" && _.ClassificationName.ToLower().Contains("" + phrase) ? _.ClassificationName : ""
                })
                .ToList();
            return Json(list);
        }

        public async Task<JsonResult> SearchItemName(string txtSearch = "", int TypeInv = 1)
        {
            decimal Quantity = 1;
            var setting = await GetListApi<PreferenceDto>(TypeId: TypeInv, Page: 1, PageSize: 1000);
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

            var list = await GetListApi<ProductDto>(TextSearch: txtSearch, Page: 1, PageSize: 1);
            if(list == null)
                list = new List<ProductDto>();

            var item = list.FirstOrDefault();
            if (item == null)
                item = new ProductDto();

            item.Quantity = Quantity;
            return Json(item);
        }

        public async Task<JsonResult> checkStock(int id)
        {
            var product = (await GetObApi<ProductDto>($"GetById?Id={id}"));
            var data = new
            {
                id = product.Id,
                name = product.Name,
                price = product.Price,
                cost = product.Cost,
                selectunitid = product.ProductUnits.FirstOrDefault(e => e.DefaultUnit).UnitId,
                selectunitName = product.ProductUnits.FirstOrDefault(e => e.DefaultUnit).UnitName,
                unitlist = product.ProductUnits,// new UnitService(User.GetSchema()).GetAllByProductId(id)
            };
            return Json(data);
        }

        public async Task<JsonResult> LoadProductsByStock(long StockId, DateTime date)
        {
            //var products = new ProductService(User.GetSchema()).GetAllByBalance(StockId, date);
            var products = await GetListApi<ProductDto>($"GetAllByBalance?StockId={StockId}&date={date}");
            var data = products.Select(e => new
            {
                id = e.Id,
                name = e.Name,
                price = e.Price,
                selectunitid = e.ProductUnits.FirstOrDefault(e => e.DefaultUnit).UnitId,
                selectunitName = e.ProductUnits.FirstOrDefault(e => e.DefaultUnit).UnitName,
                balance = e.Balance
            }).ToList();
            return Json(data);
        }
    }
}