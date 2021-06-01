using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrgSys.Controllers;
using Service.BAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace OrgSys.Areas.Sales.Controllers
{
    [Area("Sales")]
    public class InvoiceController : BaseController<InvoiceModelView>
    {
        public override void LoadViewBag(InvoiceModelView model)
        {
            ViewBag.DealerId = new SelectList(new DealerService().GetAll(model.ParentId, (int)DealerType.Client), "Id", "Name", model.DealerId);
            ViewBag.StoreId = new SelectList(new StoreService().GetAll(model.ParentId, model.StoreId), "Id", "Name", model.StoreId);
            ViewBag.PaymentTypeId = new SelectList(new PaymentTypeService().GetAll(model.ParentId, model.PaymentTypeId), "Id", "Name", model.PaymentTypeId);
            ViewBag.ProductId = new SelectList(new ProductService().GetAll(model.ParentId, 0), "Id", "Name");
        }

        public override InvoiceModelView InitializeData(InvoiceModelView ob)
        {
            if (ob == null)
                ob = new InvoiceModelView();
            ob.Date = DateTime.Now;
            return ob;
        }
        public JsonResult SearchItems(string phrase = "")
        {
            if ( phrase != null)
                phrase = phrase.Trim().ToLower();

            var itemsList = new ProductService().GetAll(phrase ,0, 0 , 1 ,10);          
            var list = itemsList.Distinct().OrderBy(_ => _.Name)
                .Select(_ => new
                {
                    _.Id,
                    _.Name,
                    _.Barcode,
                    _.Price
                })               
                .ToList();
            return Json(list);
        }

        public JsonResult checkStock(int id)
        {
            var product = new ProductService().Get(id);
            var data = new
            {
                price = product.Price,
                selectunitid = product.ProductUnits.FirstOrDefault(e=>e.DefaultUnit).UnitId,
                unitlist = new UnitService().GetAllByProductId(id)
            };           
            return Json(data);
        }        
    }
}
