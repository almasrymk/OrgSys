using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using OrgSys.Controllers;

namespace OrgSys.Areas.Setting.Controllers
{
    [Area("Setting")]
    public class SafeController : BaseController<SafeModelView>
    {

    }
}
