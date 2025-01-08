using Entity.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Entity.Model;
using Entity;

namespace Service
{
    public class AccountService : BaseOrgService<AccountModelView, Account>
    {
        public AccountService(string Schema) : base(Schema, "AccountType") { }

        public override Expression<Func<Account, bool>> CreateFilter(string textSearch, long ParentId = 0, long TypeId = 0)
        {
            return e => "" + textSearch == "" || e.Name.ToLower().Contains(textSearch.ToLower()) || e.Code.ToLower().Contains(textSearch.ToLower());
        }

        public virtual List<AccountModelView> GetById(long parentId = 0)
        {
            return repo.GetList(CreateFilter("", parentId), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Where(x => x.ParentId == parentId).Select(e => e.Map<AccountModelView>()).ToList();
        }
    }
}
