namespace Parties.Application
{
    public class SupplierProfileDto : BaseModel
    {
        public virtual long DealerId { get; set; }

        public virtual long? AccountId { get; set; }

        public virtual bool IsApproved { get; set; }

        public virtual bool IsOnHold { get; set; }
    }
}
