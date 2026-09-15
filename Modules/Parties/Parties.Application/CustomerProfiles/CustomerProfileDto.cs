namespace Parties.Application
{
    public class CustomerProfileDto : BaseModel
    {
        public virtual long DealerId { get; set; }

        public virtual long? AccountId { get; set; }

        public virtual decimal? CreditLimit { get; set; }

        public virtual bool IsCreditAllowed { get; set; }

        public virtual bool IsOnHold { get; set; }
    }
}
