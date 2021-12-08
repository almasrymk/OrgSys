using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Journal")]
    public class Journal : MovementModel
    {       
        [ForeignKey("Currency")]
        public long CurrencyId { get; set; }

        public Currency Currency { get; set; }

        public long RefranceId { get; set; }

        public long RefranceTypeId { get; set; }

        public string RefranceTable { get; set; }

        public string Note { get; set; }
    }
}