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

namespace OrgSys.Areas.Inventory.Controllers
{
    [Area("Transactions")]
    public class InventoryController : BaseController<InventoryModelView>
    {
        public override void LoadViewBag(InventoryModelView model)
        {           
            ViewBag.StoreId = new SelectList(new StoreService().GetAll(model.ParentId, 0, 1, 20), "Id", "Name", model.StoreId);
            ViewBag.ProductId = new SelectList(new ProductService().GetAll(model.ParentId, 0, 1, 20), "Id", "Name");
        }

        public override InventoryModelView InitializeData(InventoryModelView ob)
        {
            var setting = new PreferenceService();
            var StoreId = long.Parse("0" + setting.GetByKey("DefaultStore", "Inventory", ob.TypeId, 0)?.Value);

            ViewBag.NumberLine = int.Parse("0" + setting.GetByKey("NumberLine", "Inventory", ob.TypeId, 0)?.Value);
            ViewBag.AutoSave = int.Parse("0" + setting.GetByKey("AutoSave", "Inventory", ob.TypeId, 0)?.Value);
            var TypeCode = int.Parse("0" + setting.GetByKey("TypeSerial", "Inventory", ob.TypeId, 0)?.Value);
            ViewBag.TypeSerial = TypeCode;

            if (ob == null)
                ob = new InventoryModelView();

            if (ob.Id == 0)
            {
                ob.CodeNumber = new InventoryService().GetMaxCode(ob.TypeId);
                ob.Code = "" + new InventoryService().GetMaxCode(ob.TypeId);
                ob.StoreId = StoreId;
                ob.Date = DateTime.Now;
                ob.InventoryProducts = new List<InventoryProductModelView>();
              }
            
            return ob;
        }
       
    }
}
