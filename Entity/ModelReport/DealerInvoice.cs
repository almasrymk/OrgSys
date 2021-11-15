using Utility;
using Entity.Model;
using Utility.Resource;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Entity.ModelReport
{
    public class DealerInvoice 
    {


        //public DealerInvoice(Invoice invoice)
        //{
        //    this.Code = invoice.Dealer.Code;
        //    this.Name = invoice.Dealer.Name;
        //    this.Total = invoice.Net;
        //}           
        public  long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }      
        public decimal Total { get; set; }     

    }
}
