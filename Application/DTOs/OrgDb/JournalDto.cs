using System.ComponentModel.DataAnnotations.Schema;

namespace Application.DTOs
{
    public class JournalDto : MovementDto
    {
        public virtual long CurrencyId { get; set; }

        public virtual string? CurrencyName { get; set; }

        public virtual decimal Rate { get; set; }

        public virtual long RefranceId { get; set; }

        public virtual long RefranceTypeId { get; set; }

        public virtual string? RefranceTable { get; set; }

        public virtual string? Note { get; set; }
        public ICollection<JournalItemDto>? JournalItems { get; set; }
    }
}