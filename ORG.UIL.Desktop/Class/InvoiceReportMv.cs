using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORG.UIL.Desktop.Class
{
   public class InvoiceReportMv
    {       
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public string TitleCompany { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public decimal Service { get; set; }
        public decimal Tax { get; set; }
        public decimal GrandTotal { get; set; }
        public string EmpName { get; set; }
    }
}
