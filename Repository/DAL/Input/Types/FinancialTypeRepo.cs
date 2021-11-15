using Entity.Model;

namespace Repository
{
    public class FinancialTypeRepo : CurdOrg<FinancialType>
    {
        public FinancialTypeRepo(string Schema) : base(Schema) { }
    }
}