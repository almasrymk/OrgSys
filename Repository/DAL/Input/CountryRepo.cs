using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CountryRepo : CurdOrg<Country>
    {
        public CountryRepo(string Schema) : base(Schema) { }
    }
}
