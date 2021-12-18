using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class CurrencyService : BaseOrgService<CurrencyModelView, Currency>
    {
        public CurrencyService(string Schema) : base(Schema) { }
    }
}