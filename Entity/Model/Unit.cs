using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Utility;

namespace Entity.Model
{
    [Table("Unit")]
    public class Unit : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
    }
}
