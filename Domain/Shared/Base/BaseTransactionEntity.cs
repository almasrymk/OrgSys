namespace Domain.Common.Base
{
    public class BaseTransactionEntity : BaseEntity
    {
        public virtual long Serial { get; set; }

        public virtual DateTime? Date { get; set; }

        public virtual string? Notes { get; set; }
    }
}