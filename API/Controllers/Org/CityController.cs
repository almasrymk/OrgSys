using Application.Commands.Org.City.Commands;
using Application.Commands.Org.City.Queries;
using Application.Interfaces.CQRS;
using Azure;
using Domain.Shared;
using Entity.ModelView;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CityController(ISender sender) : BaseController<GetByIdQuery, SearchQuery, CreateCommand, UpdateCommand, DeleteCommand , DeleteListCommand, CityModelView>(sender)
    {

    }
}
