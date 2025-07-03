using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("GeneralProduct", Schema = "admin")]
    public class GeneralProduct : BaseEntity
    {       
        [Required]
        public virtual string Name { get; set; }

        public virtual string Nickname { get; set; }

        [Required]
        public virtual string Barcode { get; set; }

        public virtual string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Cost { get; set; }

        [ForeignKey("GeneralClassification")]
        public virtual long GeneralClassificationId { get; set; }

        public virtual GeneralClassification GeneralClassification { get; set; }      

        public virtual string Recipe { get; set; }

        public virtual ICollection<GeneralProductUnit> GeneralProductUnits { get; set; }

        public virtual ICollection<GeneralProductRecipe> GeneralProductRecipes { get; set; }

        public virtual ICollection<GeneralProductPropertyElement> GeneralProductPropertyElements { get; set; }
    }
}