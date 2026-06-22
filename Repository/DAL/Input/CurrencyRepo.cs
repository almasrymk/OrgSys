using Domain.Entities;

namespace Repository
{
    public class CurrencyRepo : CurdOrg<Currency>
    {
        public CurrencyRepo(string Schema) : base(Schema) { }
    }
}