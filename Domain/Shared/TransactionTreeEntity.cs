namespace Domain.Common
{
    public class TransactionTreeEntity : BaseTransactionEntity
    {
        public virtual string? ParentId { get; set; }
    }
}