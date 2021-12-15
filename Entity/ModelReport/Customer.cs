using Entity.Model;

namespace Entity.ModelReport
{
    public class Customer :  Dealer
    {
        public Customer(Dealer dealer)
        {
            this.Code = dealer.Code;
            this.Id = dealer.Id;
            this.TypeId = dealer.TypeId;
            this.Name = dealer.Name;
            this.Address = dealer.Address;
            this.Phone = dealer.Phone;
            this.Email = dealer.Email;
        }
    }
}