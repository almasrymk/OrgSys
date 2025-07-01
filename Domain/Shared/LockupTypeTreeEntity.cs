namespace Domain.Common
{
    public class LockupTypeTreeEntity : BaseLockupEntity
    {
        public virtual string? TypeId { get; set; }
        public virtual string? ParentId { get; set; }
    }
}