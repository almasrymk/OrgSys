using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.DAL
{
    public class OrderRepo : Curd<Order>
    {
       public long GetMaXCode(long type)
        {
            if(db.Orders.Any(e=>e.TypeId == type))
            {
                var code = db.Orders.Where(e => e.TypeId == type).Max(e => e.CodeNumber);
                return ++code;
            }
            return 1;
        }
    }
}
