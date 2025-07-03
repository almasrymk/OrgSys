using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Table")]
    public class Table : BaseEntity
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual int NumberOfPeople { get; set; }
    }
}