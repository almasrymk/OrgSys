using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class AccountTypeRepo : CurdOrg<AccountType>
    {
        public AccountTypeRepo(string Schema) : base(Schema) { }
    }
}
