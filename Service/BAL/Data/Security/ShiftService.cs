using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class ShiftService : BaseOrgService<ShiftModelView, Shift>
    {
        public ShiftService(string Schema) : base(Schema) { }
    }
}