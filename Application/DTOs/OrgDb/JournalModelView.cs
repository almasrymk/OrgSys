namespace Application.DTOs
{
    public class JournalModelView : MovementDto
    {
        public virtual long CurrencyId { get; set; }

        public virtual string CurrencyName { get; set; }

        public virtual long RefranceId { get; set; }

        public virtual long RefranceTypeId { get; set; }

        public virtual string RefranceTable { get; set; }

        public virtual string Note { get; set; }
        public ICollection<JournalItemModelView> JournalItems { get; set; }
    }
}