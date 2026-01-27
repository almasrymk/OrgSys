namespace OrgSys.Areas.Setting.Controllers
{
    using Application.Commands.Org.Setting.Property.Commands;
    using AutoMapper;
    using Entity.ModelView;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using OrgSys.Controllers;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Area("Setting")]
    public class PropertyController(IConfiguration configuration, IMapper mapper) : MainController<PropertyModelView, CreatePropertyCommand, UpdatePropertyCommand>(configuration, mapper)
    {
        public override async Task<PropertyModelView> InitializeData(PropertyModelView ob)
        {
            if (ob.PropertyElementList == null)
                ob.PropertyElementList = new List<PropertyElementModelView>();
            return ob;
        }
    }
}