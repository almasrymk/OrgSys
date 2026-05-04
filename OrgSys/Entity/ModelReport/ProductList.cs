using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.ModelReport
{
    public class ProductList 
    {
        public long Id { get; set; }
        public long ClassificationId { get; set; }
        public string ClassificationName { get; set; }
        public long ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string BarCode { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SalesPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PurchasePrice { get; set; }
        public string ImgPath { get; set; }

    }
}