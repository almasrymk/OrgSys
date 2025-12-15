using Application.Commands.Org.Country.Commands;
using Application.Commands.Org.Country.Queries;
using Application.Interfaces.CQRS;
using Azure;
using Domain.Shared;
using Entity.ModelView;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Country
{
    [ApiController]
    [Route("[controller]")]
    public class CountryController(ISender sender) : BaseController<GetByIdCountryQuery, SearchCountryQuery, CreateCountryCommand, UpdateCountryCommand, DeleteCountryCommand, DeleteListCountryCommand, CountryModelView>(sender)
    {

    }
}
