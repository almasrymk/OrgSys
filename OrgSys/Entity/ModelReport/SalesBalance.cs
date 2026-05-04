using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.ModelReport
{
    public class SalesBalance
    {

        [Key]
        public DateTime Date { get; set; }
        
  
        public decimal InAmount { get; set; }
        public decimal OutAmount { get; set; }
    
        public decimal Net { get; set; }
    }
}
