using Entity;
using System.Linq;
using Entity.Model;
using Entity.ModelView;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace Service
{
    public class TableService : BaseOrgService<TableModelView, Table>
    {
        public TableService(string Schema) : base(Schema) { }

        public override Expression<Func<Table, bool>> CreateFilter(string textSearch, long ParentId = 0, long TypeId = 0)
        {
            return e => "" + textSearch == "" || e.Name.ToLower().Contains(textSearch.ToLower()) || e.Code.ToLower().Contains(textSearch.ToLower());
        }

        public List<TableModelView> GetAllClosed(long id, long parentId = 0, long TypeId = 0)
        {
            var ids = repoAll.orderRepo.GetList(e => e.Id != id && e.CloseTable != true && e.TableId != null, null, "", Utility.Status.New).Select(e => e.TableId).Distinct().ToList();

            return repo.GetList(e => !ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => e.Map<TableModelView>()).ToList();
        }
    }
}