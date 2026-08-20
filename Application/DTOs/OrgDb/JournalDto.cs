using System.ComponentModel.DataAnnotations.Schema;

namespace Application.DTOs
{
    public class JournalDto : MovementDto
    {
        public virtual long JournalTypeId { get; set; }

        public virtual string? JournalTypeName { get; set; }

        public virtual long CurrencyId { get; set; }

        public virtual string? CurrencyName { get; set; }

        public virtual decimal Rate { get; set; }

        /// <summary>Resolved server-side from <see cref="MovementDto.Date"/>. Any client-supplied value is ignored.</summary>
        public virtual long? FiscalYearId { get; set; }

        /// <summary>Resolved server-side from <see cref="MovementDto.Date"/>. Any client-supplied value is ignored.</summary>
        public virtual long? FiscalPeriodId { get; set; }

        public virtual long RefranceId { get; set; }
        public virtual string? RefranceCode { get; set; }

        public virtual long RefranceTypeId { get; set; }

        public virtual string? RefranceTable { get; set; }

        public virtual string? Note { get; set; }
        public ICollection<JournalItemDto>? JournalItems { get; set; }
    }
}
