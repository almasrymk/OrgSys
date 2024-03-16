using Entity.Model;
using Entity.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class BankBranchService : BaseOrgService<BankBranchModelView, BankBranch>
    {
        public BankBranchService(string Schema) : base(Schema, "Bank,Country,City,District") { }

        public override Expression<Func<BankBranch, bool>> CreateFilter(string textSearch, long ParentId = 0, long TypeId = 0)
        {
            return e => "" + textSearch == "" || e.Name.ToLower().Contains(textSearch.ToLower()) || e.Code.ToLower().Contains(textSearch.ToLower());
        }

    }
}
