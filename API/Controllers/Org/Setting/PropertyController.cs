using Application.Commands.Org.Setting.Preference.Queries;
using Inventory.Application.Properties.Commands;
using Inventory.Application.Properties.Queries;
using OrgSys.SharedKernel;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class PropertyController(ISender sender) : BaseController<GetByIdPropertyQuery, SearchPropertyQuery , GetListPropertyQuery, CreatePropertyCommand, UpdatePropertyCommand, DeletePropertyCommand, DeleteListPropertyCommand, PropertyDto>(sender)
    {

    }
}