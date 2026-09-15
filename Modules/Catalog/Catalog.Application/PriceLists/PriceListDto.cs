using System.ComponentModel.DataAnnotations;

namespace Catalog.Application
{
    public class PriceListDto : BaseModel
    {
        [Required]
        public virtual string? Name { get; set; }

        public virtual long? CurrencyId { get; set; }

        public virtual DateTime? ValidFrom { get; set; }

        public virtual DateTime? ValidTo { get; set; }

        public virtual bool IsDefault { get; set; }

        public virtual bool IsActive { get; set; } = true;

        public ICollection<PriceListEntryDto>? Entries { get; set; }
    }
}
