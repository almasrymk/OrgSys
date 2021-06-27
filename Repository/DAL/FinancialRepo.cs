using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.DAL
{
    public class FinancialRepo : Curd<Financial>
    {
       public long GetMaXCode(long type)
        {
            if(db.Financials.Any(e=>e.TypeId == type))
            {
                var code = db.Financials.Where(e => e.TypeId == type).Max(e => e.CodeNumber);
                return ++code;
            }
            return 1;
        }
    }
}
