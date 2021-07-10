using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.DAL
{
    public class ProductRepo : Curd<Product>
    {
        public long GetMaXCode(long type)
        {
            if (db.Products.Any())
            {
                var code = db.Products.Max(e => e.CodeNumber);
                return ++code;
            }
            return 1;
        }
    }
}
