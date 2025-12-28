using Application.Commands.Org.Setting.City.Queries;
using Application.Commands.Org.Setting.Country.Commands;
using Application.Commands.Org.Setting.Country.Queries;
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
    public class CountryController(ISender sender) : BaseController<GetByIdCountryQuery, SearchCountryQuery , GetListCountryQuery, CreateCountryCommand, UpdateCountryCommand, DeleteCountryCommand, DeleteListCountryCommand, CountryModelView>(sender)
    {

    }
}