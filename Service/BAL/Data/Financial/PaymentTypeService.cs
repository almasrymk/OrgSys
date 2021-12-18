using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class PaymentTypeService : BaseOrgService<PaymentTypeModelView, PaymentType>
    {
        public PaymentTypeService(string Schema) : base(Schema) { }
    }
}