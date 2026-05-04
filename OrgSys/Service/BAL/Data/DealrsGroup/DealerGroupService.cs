using Entity.Model;
using Entity.ModelView;
using System.Linq.Expressions;
using System;
using System.Net.Http.Headers;

namespace Service
{
    public class DealerGroupService : BaseOrgService<DealerGroupModelView, DealerGroup>
    {
        public DealerGroupService(string Schema) : base(Schema) { }

        public override Expression<Func<DealerGroup, bool>> CreateFilter(string textSearch, long ParentId = 0, long TypeId = 0)
        {
            return e => ("" + textSearch == "" || e.Name.ToLower().Contains(textSearch.ToLower()) || e.Code.ToLower().Contains(textSearch.ToLower())) && (TypeId == 0 || e.TypeId == TypeId) && (ParentId == 0 || e.ParentId == ParentId);
        }
    }
}
