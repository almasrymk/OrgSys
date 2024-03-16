using Entity;
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
    public class DistrictService : BaseOrgService<DistrictModelView, District>
    {
        public DistrictService(string Schema) : base(Schema, "Country,City") { }

        public override Expression<Func<District, bool>> CreateFilter(string textSearch, long ParentId = 0, long TypeId = 0)
        {
            return e => "" + textSearch == "" || e.Name.ToLower().Contains(textSearch.ToLower()) || e.Code.ToLower().Contains(textSearch.ToLower());
        }

        public virtual List<DistrictModelView> GetById(long parentId = 0, long TypeId = 0)
        {
            return repo.GetList(CreateFilter("", parentId, TypeId), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => e.Map<DistrictModelView>()).ToList();
        }
    }
}
