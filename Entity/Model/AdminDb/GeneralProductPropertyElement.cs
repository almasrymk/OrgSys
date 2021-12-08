using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("GeneralProductPropertyElement", Schema = "admin")]
   public class GeneralProductPropertyElement : BaseModel
    {
        [ForeignKey("GeneralProduct")]
        public long? GeneralProductId { get; set; }

        public GeneralProduct GeneralProduct { get; set; }

        [ForeignKey("GeneralProperty")]
        public long? GeneralPropertyId { get; set; }

        public GeneralProperty GeneralProperty { get; set; }

        [ForeignKey("GeneralPropertyElement")]
        public long? GeneralPropertyElementId { get; set; }

        public GeneralPropertyElement GeneralPropertyElement { get; set; }

        public bool IsChecked { get; set; }
    }
}