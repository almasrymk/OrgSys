namespace Domain.Common.Base
{
    public class BaseTreeEntity: BaseEntity
    {
        public virtual string? ParentId { get; set; }
    }
}