using System.ComponentModel.DataAnnotations;

namespace Treasury.Application
{
    public class BankDto : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string? Name { get; set; }

        public long? CountryId { get; set; }
    }
}