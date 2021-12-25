using System;
using System.Collections.Generic;
using System.Linq;
using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrgSys.Controllers;
using Service;

namespace OrgSys.Areas.Setting.Controllers
{
    [Area("Setting")]
    public class ProductController : BaseController<ProductModelView>
    {        
        public override void LoadViewBag(ProductModelView model)
        {
            ViewBag.UnitList = new SelectList(new UnitService(User.GetSchema()).GetAll(model.ParentId, model.TypeId), "Id", "Name");
        }

        public override ProductModelView InitializeData(ProductModelView ob)
        {
            if (ob.ProductUnits == null)
                 ob.ProductUnitList = new List<ProductUnitModelView>();
            if (ob.Id == 0)
            {
                ob.CodeNumber = new ProductService(User.GetSchema()).GetMaxCode(ob.TypeId);
                ob.Code = "" + new ProductService(User.GetSchema()).GetMaxCode(ob.TypeId);
            }
            ob.ClassificationName = new ClassificationService(User.GetSchema()).Get(ob.ClassificationId)?.Name;
            ob.DealerName = new DealerService(User.GetSchema()).Get(ob.DealerId??1)?.Name;
            return ob;
        }

        public JsonResult GetList(int ProductId)
        {
            var ProductUnitList = new SelectList(new ProductUnitService(User.GetSchema()).GetAll(0,0).Where(e=>e.ProductId==ProductId).Select(e=>e.UnitName));
            return Json(new { success = true, ProductUnitList });
        }

        public ActionResult SearchProducts(string txt = "", int page = 1 , int Type = 1 , int index = 0)
        {
            ViewBag.index = index;
            var list = new ProductService(User.GetSchema()).GetAll(txt, 0, 0, page, 7);
            return Type != 1 ? (ActionResult)PartialView("SearchProductsList", list) : View("SearchProducts", list);
        }

        public JsonResult SearchItems(string phrase = "", int TypeInv = 1)
        {
            decimal Quantity = 1;
            var setting = new PreferenceService(User.GetSchema());
            if (phrase == null)
                phrase = "";
            phrase = phrase.Trim().ToLower();

            var CodeElectronicScale = setting.GetByKey("CodeElectronicScale", "Invoice", TypeInv, 0)?.Value;
            var LengthElectronicScale = int.Parse(setting.GetByKey("LengthElectronicScale", "Invoice", TypeInv, 0)?.Value);
            var LengthQtyElectronicScale = int.Parse(setting.GetByKey("LengthQtyElectronicScale", "Invoice", TypeInv, 0)?.Value);

            if (phrase.Length >= LengthElectronicScale &&  "" + CodeElectronicScale != "" && "" + CodeElectronicScale != "0" && "" + LengthElectronicScale != "" && "" + LengthElectronicScale != "0")
            {
                if (phrase.StartsWith(CodeElectronicScale))
                {
                    var code = phrase.Substring(0, LengthElectronicScale);
                    var qty = phrase.Substring(LengthElectronicScale);
                    Quantity = decimal.Parse("0" + qty) / 1000;
                    phrase = code;
                }
            }

            var itemsList = new ProductService(User.GetSchema()).GetAll(phrase, 0, 0, 1, 10);
            var list = itemsList.Distinct().OrderBy(_ => _.Name)
                .Select(_ => new
                {
                    _.Id,
                    _.Name,
                    Barcode = phrase == _.Barcode ? Utility.Resource.Title_Designer.Barcode + " " + _.Barcode : "",
                    _.Price,
                    _.Cost,
                    quantity = Quantity,
                     Code = phrase == _.Code ? Utility.Resource.Title_Designer.Code + " " +  _.Code : "",
                    ClassificationName = "" + phrase != "" && _.ClassificationName.ToLower().Contains("" + phrase) ? _.ClassificationName : ""
                })
                .ToList();
            return Json(list);
        }

        public JsonResult SearchItemName(string txtSearch = "", int TypeInv = 1)
        {
            var setting = new PreferenceService(User.GetSchema());
            decimal Quantity = 1;
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();

            var CodeElectronicScale = setting.GetByKey("CodeElectronicScale", "Invoice", TypeInv, 0)?.Value;
            var LengthElectronicScale = int.Parse(setting.GetByKey("LengthElectronicScale", "Invoice", TypeInv, 0)?.Value);
            var LengthQtyElectronicScale = int.Parse(setting.GetByKey("LengthQtyElectronicScale", "Invoice", TypeInv, 0)?.Value);

            if (txtSearch.Length >= LengthElectronicScale && "" + CodeElectronicScale != "" && "" + CodeElectronicScale != "0" && "" + LengthElectronicScale != "" && "" + LengthElectronicScale != "0") 
            {
                if (txtSearch.StartsWith(CodeElectronicScale))
                {
                    var code = txtSearch.Substring(0, LengthElectronicScale);
                    var qty = txtSearch.Substring( LengthElectronicScale);
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

        public JsonResult checkStock(int id)
        {
            var product = new ProductService(User.GetSchema()).Get(id);
            var data = new
            {
                id = product.Id,
                name = product.Name,
                price = product.Price,
                cost = product.Cost,
                selectunitid = product.ProductUnits.FirstOrDefault(e => e.DefaultUnit).UnitId,
                selectunitName = product.ProductUnitList.FirstOrDefault(e => e.DefaultUnit).UnitName,
                unitlist = new UnitService(User.GetSchema()).GetAllByProductId(id)
            };
            return Json(data);
        }

        public JsonResult LoadProductsByStore(long storeId , DateTime date)
        {
            var products = new ProductService(User.GetSchema()).GetAllByBalance(storeId , date);
            var data = products.Select(e=> new
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