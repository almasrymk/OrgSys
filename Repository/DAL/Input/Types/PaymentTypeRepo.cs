using Domain.Entities;

namespace Repository
{
    public class PaymentTypeRepo : CurdOrg<PaymentType>
    {
        public PaymentTypeRepo(string Schema) : base(Schema) { }
    }
}