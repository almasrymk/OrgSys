using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.DAL
{
    public class DealerRepo : Curd<Dealer>
    {
        public long GetMaXCode(long type)
        {
            if (db.Dealers.Any(e => e.TypeId == type))
            {
                var code = db.Dealers.Where(e => e.TypeId == type).Max(e => e.CodeNumber);
                return ++code;
            }
            return 1;
        }
    }
}
