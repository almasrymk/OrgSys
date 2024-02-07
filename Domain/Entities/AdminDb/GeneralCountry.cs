using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("GeneralCountry", Schema = "admin")]
    public class GeneralCountry : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string Name { get; set; }      
    }
}