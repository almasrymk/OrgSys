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
            ViewBag.DealerId = new SelectList(new DealerService().GetAll(model.ParentId, model.TypeId == 1 || model.TypeId == 3 ? (int)DealerType.Client : (int)DealerType.Supplier), "Id", "Name");
            ViewBag.StoreId = new SelectList(new StoreService().GetAll(model.ParentId, 0), "Id", "Name");
            ViewBag.ToStoreId = new SelectList(new StoreService().GetAll(model.ParentId, 0), "Id", "Name");
            ViewBag.ProductId = new SelectList(new ProductService().GetAll(model.ParentId, 0), "Id", "Name");          
        }

        public override InventoryModelView InitializeData(InventoryModelView ob)
        {
            var setting = new PreferenceService();
            var StoreId = long.Parse("0" + setting.GetByKey("DefaultStore", "Inventory", ob.TypeId, 0)?.Value);

            long DealerId = 0;
            if(ob.TypeId == 1)
                DealerId = long.Parse("0" + setting.GetByKey("DefaultSupplier", "Inventory", ob.TypeId, 0)?.Value);
            else if (ob.TypeId == 2)
                DealerId = long.Parse("0" + setting.GetByKey("DefaultCustomer", "Inventory", ob.TypeId, 0)?.Value);

            ViewBag.NumberLine = int.Parse("0" + setting.GetByKey("NumberLine", "Inventory", ob.TypeId, 0)?.Value);
            ViewBag.OrderTabe = int.Parse("0" + setting.GetByKey("OrderTabe", "Inventory", ob.TypeId, 0)?.Value);
            ViewBag.AutoSave = int.Parse("0" + setting.GetByKey("AutoSave", "Inventory", ob.TypeId, 0)?.Value);
            var TypeCode = int.Parse("0" + setting.GetByKey("TypeSerial", "Inventory", ob.TypeId, 0)?.Value);
            ViewBag.TypeSerial = TypeCode;
            ViewBag.AllowRepeated = int.Parse("0" + setting.GetByKey("AllowRepeated", "Inventory", ob.TypeId, 0)?.Value);

            if (ob == null)
                ob = new InventoryModelView();

            if (ob.Id == 0)
            {
                ob.CodeNumber = new InventoryService().GetMaxCode(ob.TypeId);
                ob.Code = "" + new InventoryService().GetMaxCode(ob.TypeId);
                ob.Date = DateTime.Now;
                //ob.InventoryProducts = new List<InventoryProductModelView>();
            }
            
            return ob;
        }
       
    }
}
