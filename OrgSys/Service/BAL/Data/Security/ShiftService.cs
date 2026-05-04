using Entity.Model;
using Entity.ModelView;
using System.Linq.Expressions;
using System;

namespace Service
{
    public class ShiftService : BaseOrgService<ShiftModelView, Shift>
    {
        public ShiftService(string Schema) : base(Schema) { }

        public override Expression<Func<Shift, bool>> CreateFilter(string textSearch, long ParentId = 0, long TypeId = 0)
        {
            return e => "" + textSearch == "" || e.Name.ToLower().Contains(textSearch.ToLower()) || e.Code.ToLower().Contains(textSearch.ToLower());
        }
    }
}