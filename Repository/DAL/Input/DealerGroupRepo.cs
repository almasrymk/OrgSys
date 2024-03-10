using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class DealerGroupRepo : CurdOrg<DealerGroup>
    {
        public DealerGroupRepo(string Schema) : base(Schema) { }

    }
}
