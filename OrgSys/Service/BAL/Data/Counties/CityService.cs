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
    public class CityService : BaseOrgService<CityModelView, City>
    {
        public CityService(string Schema) : base(Schema, "Country") { }

        public override Expression<Func<City, bool>> CreateFilter(string textSearch, long ParentId = 0, long TypeId = 0)
        {
            return e => "" + textSearch == "" || e.Name.ToLower().Contains(textSearch.ToLower()) || e.Code.ToLower().Contains(textSearch.ToLower());
        }

        public virtual List<CityModelView> GetById(long parentId = 0)
        {
            return repo.GetList(CreateFilter("", parentId), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Where(x=>x.CountryId== parentId).Select(e => e.Map<CityModelView>()).ToList();
        }
    }
}
