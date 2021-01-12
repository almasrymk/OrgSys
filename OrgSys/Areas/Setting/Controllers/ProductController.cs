using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrgSys.Controllers;
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
            ViewBag.UnitId = new SelectList(new UnitService().GetAll(model.ParentId, (int)DealerType.Supplier), "Id", "Name");
        }

        public override ProductModelView InitializeData(ProductModelView ob)
        {
            if (ob.ProductUnits == null)
                 ob.ProductUnits = new List<ProductUnitModelView>();
            ob.ProductUnits = new List<ProductUnitModelView> { new ProductUnitModelView { Id = -1 } };
            return ob;
        }
    }
}
