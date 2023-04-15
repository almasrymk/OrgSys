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
            var StockId = long.Parse("0" + setting.GetByKey("DefaultStock", "Inventory", ob.TypeId, 0)?.Value);           
            ViewBag.AutoSave = int.Parse("0" + setting.GetByKey("AutoSave", "Inventory", ob.TypeId, 0)?.Value);
            var TypeCode = int.Parse("0" + setting.GetByKey("TypeSerial", "Inventory", ob.TypeId, 0)?.Value);
            ViewBag.TypeSerial = TypeCode;

            if (ob == null)
                ob = new InventoryModelView();

            if (ob.Id == 0)
            {
                ob.CodeNumber = new InventoryService(User.GetSchema()).GetMaxCode(ob.TypeId);
                ob.Code = "" + ob.CodeNumber;
                ob.StockId = StockId;
                ob.Date = DateTime.Now;
                ob.InventoryProductList = new List<InventoryProductModelView>();
              }
            ob.StockName = new StockService(User.GetSchema()).Get(ob.StockId??0).Name;
            return ob;
        }       
    }
}