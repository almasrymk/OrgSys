namespace OrgSys.Areas.Setting.Controllers
{
    using global::Inventory.Application.Properties.Commands;
    using AutoMapper;
    using Application.DTOs;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using OrgSys.Controllers;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Area("Setting")]
    public class PropertyController(IConfiguration configuration, IMapper mapper) : MainController<PropertyDto, CreatePropertyCommand, UpdatePropertyCommand>(configuration, mapper)
    {
        public override async Task<PropertyDto> InitializeData(PropertyDto ob)
        {
            if (ob.PropertyElementList == null)
                ob.PropertyElementList = new List<PropertyElementDto>();
            return ob;
        }
    }
}