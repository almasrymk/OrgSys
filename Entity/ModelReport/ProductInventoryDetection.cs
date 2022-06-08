namespace Entity.ModelReport
{
    public class ProductInventoryDetection
    {
        public long Id { get; set; }
        public long ClassificationId { get; set; }
        public string ClassificationName { get; set; }
        public long ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public long StoreId { get; set; }
        public string StoreName { get; set; }
        public decimal ActualBalance { get; set; }
        public decimal Balance { get; set; }
    }
}