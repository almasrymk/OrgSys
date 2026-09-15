using Catalog.Application.Attributes.Commands;
using Catalog.Application.Attributes.Queries;
using OrgSys.SharedKernel;
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