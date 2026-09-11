namespace Reporting.Application
{
    public class CategoryList
    {
        public long Id { get; set; }
        public long ClassificationId { get; set; }
        public string? ClassificationName { get; set; }
        public int CountProduct { get; set; }
        public virtual bool BePurchased { get; set; }

        public virtual bool BeSold { get; set; }

        public virtual bool BeManufactured { get; set; }
    }
}