using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Utility;

namespace Entity.Model
{
    [Table("Product")]
    public class Product : BaseModel
    {
        public Product()
        {
            ProductUnits = new HashSet<ProductUnit>();
        }

        public long CodeNumber { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 3)]
        public string Code { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(15, MinimumLength = 3)]
        public string Nickname { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 5)]
        public string Barcode { get; set; }

        [StringLength(500, MinimumLength = 5)]
        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal Cost { get; set; }

        [Required]
        public long ClassificationId { get; set; }

        public virtual Classification Classification { get; set; }

        public long? DealerId { get; set; }

        public virtual Dealer Dealer { get; set; }

        public string Recipe { get; set; }
        public ICollection<ProductUnit> ProductUnits { get; set; }
    }
}