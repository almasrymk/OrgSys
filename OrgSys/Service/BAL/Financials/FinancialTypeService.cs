using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class FinancialTypeService : BaseOrgService<FinancialTypeModelView, FinancialType>
    {
        public FinancialTypeService(string Schema) : base(Schema) { }
    }
}