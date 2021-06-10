using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Utility;

namespace Entity.Model
{
    [Table("InvoiceType")]
    public class InvoiceType : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
        public int InOut { get; set; }
        public string Icon { get; set; }
        public string Group { get; set; }
    }
}
