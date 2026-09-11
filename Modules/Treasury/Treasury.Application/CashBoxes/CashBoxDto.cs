using System.ComponentModel.DataAnnotations;

namespace Treasury.Application
{
    public class CashBoxDto : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string? Name { get; set; }

        public virtual long? AccountId { get; set; }

        public virtual string? AccountName { get; set; }
    }
}
