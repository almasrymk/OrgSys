namespace Domain.Common.Base
{
    public class BaseEntity
    {
        public virtual Guid Id { get; set; } = Guid.NewGuid();

        public virtual string? MaskText { get; set; }

        public virtual bool Hide { get; set; }

        public virtual Status Status { get; set; }
    }
}