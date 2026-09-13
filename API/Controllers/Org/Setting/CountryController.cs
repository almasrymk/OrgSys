using MasterData.Application.Cities.Queries;
using MasterData.Application.Countries.Commands;
using MasterData.Application.Countries.Queries;
using OrgSys.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class CountryController(ISender sender) : BaseController<GetByIdCountryQuery, SearchCountryQuery , GetListCountryQuery, CreateCountryCommand, UpdateCountryCommand, DeleteCountryCommand, DeleteListCountryCommand, CountryDto>(sender)
    {

    }
}