using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Nationality", Schema = "admin")]
    public class Nationality : BaseModel
    {
        [Required]
        public string Name { get; set; }       
    }
}