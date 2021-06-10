using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.DAL
{
    public class TransactionRepo : Curd<Transaction>
    {
       public long GetMaXCode(long type)
        {
            if(db.Transactions.Any(e=>e.TypeId == type))
            {
                var code = db.Transactions.Where(e => e.TypeId == type).Max(e => e.CodeNumber);
                return ++code;
            }
            return 1;
        }
    }
}
