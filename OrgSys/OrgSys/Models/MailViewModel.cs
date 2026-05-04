using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OrgSys
{
    public class MailViewModel
    {
        public string Sender { get; set; }
        public string Receiver  { get; set; }
        public string Date { get; set; }
        public string Url { get; set; }
        public string BaseUrl { get; set; }        
        public string TechnicalSupportUrl { get; set; }        
        public string LoginUrl { get; set; }
    }
}
