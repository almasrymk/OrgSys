using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using OrgSys.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgSys.Areas.Sales.Controllers
{
    [Area("Sales")]
    public class ReturnInvoiceController : BaseController<InvoiceModelView>
    {
       
    }
}
