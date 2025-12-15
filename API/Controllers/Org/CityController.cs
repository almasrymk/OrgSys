using Application.Commands.Org.City.Commands;
using Application.Commands.Org.City.Queries;
using Application.Interfaces.CQRS;
using Azure;
using Domain.Shared;
using Entity.ModelView;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.City
{
    [ApiController]
    [Route("[controller]")]
    public class CityController(ISender sender) : BaseController<GetByIdCityQuery, SearchCityQuery, CreateCityCommand, UpdateCityCommand, DeleteCityCommand , DeleteListCityCommand, CityModelView>(sender)
    {

    }
}
