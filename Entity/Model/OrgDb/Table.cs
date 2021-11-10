using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Table", Schema = "org")]
    public class Table : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        public string Description { get; set; }

        public int NumberOfPeople { get; set; }
    }
}