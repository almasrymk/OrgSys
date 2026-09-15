using System.ComponentModel.DataAnnotations.Schema;

namespace Catalog.Application
{
    public class PriceListEntryDto : BaseModel
    {
        public virtual long PriceListId { get; set; }

        public virtual long ProductId { get; set; }

        public virtual long? UnitId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Price { get; set; }

        public virtual decimal? MinQuantity { get; set; }

        public virtual DateTime? ValidFrom { get; set; }

        public virtual DateTime? ValidTo { get; set; }

        public virtual bool IsActive { get; set; } = true;

        public virtual string? ProductName { get; set; }

        public virtual string? UnitName { get; set; }
    }
}
