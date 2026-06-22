using Domain.Entities;

namespace Repository
{
    public class TableRepo : CurdOrg<Table>
    {
        public TableRepo(string Schema) : base(Schema) { }
    }
}