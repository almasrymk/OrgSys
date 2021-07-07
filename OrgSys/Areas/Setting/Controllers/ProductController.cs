using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrgSys.Controllers;
using Repository;
using Service.BAL;
using Utility;

namespace OrgSys.Areas.Setting.Controllers
{
    [Area("Setting")]
    public class ProductController : BaseController<ProductModelView>
    {
        
        public override void LoadViewBag(ProductModelView model)
        {
            ViewBag.ClassificationId = new SelectList(new ClassificationService().GetAll(model.ParentId , model.TypeId), "Id", "Name" , model.ClassificationId);
            ViewBag.DealerId = new SelectList(new DealerService().GetAll(model.ParentId, (int)DealerType.Supplier), "Id", "Name", model.DealerId);
            ViewBag.UnitList = new SelectList(new UnitService().GetAll(model.ParentId, model.TypeId), "Id", "Name");
            ViewBag.ProductList = new SelectList(new ProductService().GetAll(model.ParentId, model.TypeId), "Id", "Name");

        }

        public override ProductModelView InitializeData(ProductModelView ob)
        {
            if (ob.ProductUnits == null)
                 ob.ProductUnits = new List<ProductUnitModelView>();         
            return ob;
        }
        public JsonResult GetList(int ProductId)
        {
            var ProductUnitList = new SelectList(new ProductUnitService().GetAll(0,0).Where(e=>e.ProductId==ProductId).Select(e=>e.UnitName));
            return Json(new { success = true, ProductUnitList });
        }

        public ActionResult SearchProducts(string txt = "", int page = 1 , int Type = 1 , int index = 0)
        {
            ViewBag.index = index;
            var list = new ProductService().GetAll(txt, 0, 0, page, 7);
            return Type != 1 ? (ActionResult)PartialView("SearchProductsList", list) : View("SearchProducts", list);
        }

        public JsonResult SearchItems(string phrase = "")
        {
            if (phrase != null)
                phrase = phrase.Trim().ToLower();

            var itemsList = new ProductService().GetAll(phrase, 0, 0, 1, 10);
            var list = itemsList.Distinct().OrderBy(_ => _.Name)
                .Select(_ => new
                {
                    _.Id,
                    _.Name,
                    _.Barcode,
                    _.Price,
                    _.Cost,
                })
                .ToList();
            return Json(list);
        }

        public JsonResult checkStock(int id)
        {
            var product = new ProductService().Get(id);
            var data = new
            {
                id = product.Id,
                name = product.Name,
                price = product.Price,
                selectunitid = product.ProductUnits.FirstOrDefault(e => e.DefaultUnit).UnitId,
                selectunitName = product.ProductUnits.FirstOrDefault(e => e.DefaultUnit).UnitName,
                unitlist = new UnitService().GetAllByProductId(id)
            };
            return Json(data);
        }

        public JsonResult LoadProductsByStore(long storeId)
        {
            var products = new ProductService().GetAllByBalance(storeId);
            var data = products.Select(e=> new
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