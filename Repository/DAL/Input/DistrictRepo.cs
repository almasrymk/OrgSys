using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class DistrictRepo :CurdOrg<District>
    {
        public DistrictRepo(string Schema) : base(Schema) { }
    }
}
