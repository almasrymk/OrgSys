using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Shift")]
    public class Shift : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        public TimeSpan Start { get; set; }

        public TimeSpan End { get; set; }
    }
}