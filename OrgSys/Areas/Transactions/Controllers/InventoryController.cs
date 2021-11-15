using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using OrgSys.Controllers;
using Service;
using System;
using System.Collections.Generic;

namespace OrgSys.Areas.Inventory.Controllers
{
    [Area("Transactions")]
    public class InventoryController : BaseController<InventoryModelView>
    {       
        public override InventoryModelView InitializeData(InventoryModelView ob)
        {
            var setting = new PreferenceService(User.GetSchema());
            var StoreId = long.Parse("0" + setting.GetByKey("DefaultStore", "Inventory", ob.TypeId, 0)?.Value);           
            ViewBag.AutoSave = int.Parse("0" + setting.GetByKey("AutoSave", "Inventory", ob.TypeId, 0)?.Value);
            var TypeCode = int.Parse("0" + setting.GetByKey("TypeSerial", "Inventory", ob.TypeId, 0)?.Value);
            ViewBag.TypeSerial = TypeCode;

            if (ob == null)
                ob = new InventoryModelView();

            if (ob.Id == 0)
            {
                ob.CodeNumber = new InventoryService().GetMaxCode(ob.TypeId);
                ob.Code = "" + ob.CodeNumber;
                ob.StoreId = StoreId;
                ob.Date = DateTime.Now;
                ob.InventoryProducts = new List<InventoryProductModelView>();
              }
            ob.StoreName = new StoreService(User.GetSchema()).Get(ob.StoreId??0).Name;
            return ob;
        }       
    }
}