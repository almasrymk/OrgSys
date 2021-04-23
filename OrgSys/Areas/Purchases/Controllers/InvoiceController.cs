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

namespace OrgSys.Areas.Purchases.Controllers
{
    [Area("Purchases")]
    public class InvoiceController : BaseController<InvoiceModelView>
    {
        public override void LoadViewBag(InvoiceModelView model)
        {
            ViewBag.DealerId = new SelectList(new DealerService().GetAll(model.ParentId, (int)DealerType.Client), "Id", "Name", model.DealerId);
            ViewBag.StoreId = new SelectList(new StoreService().GetAll(model.ParentId, model.StoreId), "Id", "Name");
            ViewBag.PaymentTypeId = new SelectList(new PaymentTypeService().GetAll(model.ParentId, model.PaymentTypeId), "Id", "Name");
        }
    }
}
