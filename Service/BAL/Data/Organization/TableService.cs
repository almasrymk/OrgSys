using Entity;
using System.Linq;
using Entity.Model;
using Entity.ModelView;
using System.Collections.Generic;

namespace Service
{
    public class TableService : BaseOrgService<TableModelView, Table>
    {
        public TableService(string Schema) : base(Schema) { }

        public List<TableModelView> GetAllClosed(long id, long parentId = 0, long TypeId = 0)
        {
            var ids = repoAll.orderRepo.GetList(e => e.Id != id && e.CloseTable != true && e.TableId != null, null, "", Utility.Status.New).Select(e => e.TableId).Distinct().ToList();

            return repo.GetList(e => !ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => e.Map<TableModelView>()).ToList();
        }
    }
}