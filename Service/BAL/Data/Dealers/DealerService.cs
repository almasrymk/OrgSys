using Entity.Model;
using Entity.ModelView;
using System.Linq.Expressions;
using System;

namespace Service
{
    public class DealerService : BaseOrgService<DealerModelView, Dealer>
    {
        public DealerService(string Schema) : base(Schema , "DealerGroup,Country,City,District") { }

        public override Expression<Func<Dealer, bool>> CreateFilter(string textSearch, long ParentId = 0, long TypeId = 0)
        {
            return e => "" + textSearch == "" || e.Name.ToLower().Contains(textSearch.ToLower()) || e.Code.ToLower().Contains(textSearch.ToLower());
        }
    }
}