using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Shift")]
    public class Shift : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string Name { get; set; }

        public virtual TimeSpan Start { get; set; }

        public virtual TimeSpan End { get; set; }
    }
}