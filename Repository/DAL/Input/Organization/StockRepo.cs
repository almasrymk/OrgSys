using Entity.Model;

namespace Repository
{
    public class StockRepo : CurdOrg<Stock>
    {
        public StockRepo(string Schema) : base(Schema) { }
    }
}