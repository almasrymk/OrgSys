using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.DAL
{
    public class InvoiceRepo : Curd<Invoice>
    {
       public long GetMaXCode(long type)
        {
            if(db.Invoices.Any(e=>e.TypeId == type))
            {
                var code = db.Invoices.Where(e => e.TypeId == type).Max(e => e.CodeNumber);
                return ++code;
            }
            return 1;
        }
    }
}
