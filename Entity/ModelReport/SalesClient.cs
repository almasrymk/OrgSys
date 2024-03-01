using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.ModelReport
{
    public class SalesClient
    {

        [Key]
        public long DealerId { get; set; }
        public string DealerName { get; set; }

   


        public decimal InAmount { get; set; }
        public decimal OutAmount { get; set; }
      
        public decimal Net { get; set; }
    }
}
