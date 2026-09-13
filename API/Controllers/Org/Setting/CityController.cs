using MasterData.Application.Cities.Commands;
using MasterData.Application.Cities.Queries;
using OrgSys.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class CityController(ISender sender) : BaseController<GetByIdCityQuery, SearchCityQuery , GetListCityQuery , CreateCityCommand, UpdateCityCommand, DeleteCityCommand , DeleteListCityCommand, CityDto>(sender)
    {
        [HttpGet("GetListByCountryId")]
        public virtual async Task<IActionResult> GetListByCountryId(string? KeySearch , long? CountryId, long ParentId, long TypeId, int Page, int PageSize, CancellationToken cancellationToken)
        {
            var query = (GetListByCountryCityQuery)Activator.CreateInstance(typeof(GetListByCountryCityQuery), KeySearch , CountryId , ParentId, TypeId, Page, PageSize)!;
            var res = await Sender.Send(query, cancellationToken);
            return res.StatusCode == HttpStatusCode.OK ? Ok(res) : BadRequest(res);
        }
    }
}