using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class TableService : BaseOrgService<TableModelView, Table>
    {
        public TableService(string Schema) : base(Schema) { }
    }
}