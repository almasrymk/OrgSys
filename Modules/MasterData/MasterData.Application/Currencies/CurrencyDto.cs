using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterData.Application
{
    public class CurrencyDto : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public string? Name { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Rate { get; set; }

        public bool IsDefault { get; set; }
    }
}