namespace Domain.Common
{
    public class TransactionTypeTreeEntity : BaseTransactionEntity
    {
        public virtual string? TypeId { get; set; }
        public virtual string? ParentId { get; set; }
    }
}