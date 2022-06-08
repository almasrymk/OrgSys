namespace Entity.ModelReport
{
    public class ProductCost
    {
        public long Id { get; set; }
        public long ClassificationId { get; set; }
        public string ClassificationName { get; set; }
        public long ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public decimal CurrentCost { get; set; }
        public decimal AvgCost { get; set; }
        public decimal MinCost { get; set; }
        public decimal MaxCost { get; set; }
        public decimal FirstCost { get; set; }
        public decimal LastCost { get; set; }
    }
}