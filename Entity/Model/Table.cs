using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Utility;

namespace Entity.Model
{
    [Table("Table")]
    public class Table : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfPeople { get; set; }
    }
}
