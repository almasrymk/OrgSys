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

        public JsonResult SearchItems(string phrase = "")
        {
            if (phrase != null)
                phrase = phrase.Trim().ToLower();

            var itemsList = new ProductService().GetAll(0,0);
            if (phrase != "*")
            {
                itemsList = itemsList.Where(_ => _.Name.StartsWith(phrase)).ToList();

            }
            var list = itemsList.Distinct().OrderBy(_ => _.Name)
                .Select(_ => new
                {
                    _.Id,
                    _.Name,
                    _.Barcode,
                    _.Price
                })
                .Take(20)
                .ToList();
            return Json(list);
        }

        public JsonResult checkStock(int id)
        {

            var data = new ProductService().GetAll(0, 0).Select(_ => new
            {
                qty = _.Price
            }).FirstOrDefault();

            return Json(data);
        }
    }
}
