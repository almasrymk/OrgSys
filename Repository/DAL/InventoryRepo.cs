using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.DAL
{
    public class InventoryRepo : Curd<Inventory>
    {
       public long GetMaXCode(long type)
        {
            if(db.Inventories.Any(e=>e.TypeId == type))
            {
                var code = db.Inventories.Where(e => e.TypeId == type).Max(e => e.CodeNumber);
                return ++code;
            }
            return 1;
        }
    }
}
