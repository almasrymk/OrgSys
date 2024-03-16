using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class BankRepo : CurdOrg<Bank>
    {
        public BankRepo(string Schema) : base(Schema) { }
    }
}
