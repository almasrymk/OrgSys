using Application.Commands.Org.Setting.Preference.Queries;
using Application.Commands.Org.Setting.Property.Commands;
using Application.Commands.Org.Setting.Property.Queries;
using Application.Interfaces.CQRS;
using Azure;
using Domain.Shared;
using Entity.ModelView;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class PropertyController(ISender sender) : BaseController<GetByIdPropertyQuery, SearchPropertyQuery , GetListPropertyQuery, CreatePropertyCommand, UpdatePropertyCommand, DeletePropertyCommand, DeleteListPropertyCommand, PropertyModelView>(sender)
    {

    }
}