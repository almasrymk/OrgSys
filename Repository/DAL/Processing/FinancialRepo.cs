using Entity.Model;

namespace Repository
{
    public class FinancialRepo : CurdOrg<Financial>
    {
        public FinancialRepo(string Schema) : base(Schema) { }
    }
}