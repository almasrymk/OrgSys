using Application.Commands.Org.Setting.City.Commands;
using Application.Commands.Org.Setting.City.Queries;
using Application.Interfaces.CQRS;
using Azure;
using Domain.Shared;
using Entity.ModelView;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class CityController(ISender sender) : BaseController<GetByIdCityQuery, SearchCityQuery , GetListCityQuery , CreateCityCommand, UpdateCityCommand, DeleteCityCommand , DeleteListCityCommand, CityModelView>(sender)
    {
        [HttpGet("GetListByCountryId")]
        public virtual async Task<IActionResult> GetListByCountryId(string? KeySearch , long ParentId, long TypeId, int Page, int PageSize, CancellationToken cancellationToken)
        {
            var query = (GetListCityQuery)Activator.CreateInstance(typeof(GetListCityQuery), KeySearch , ParentId, TypeId, Page, PageSize)!;
            var res = await sender.Send(query, cancellationToken);
            return res.StatusCode == HttpStatusCode.OK ? Ok(res) : BadRequest(res);
        }
    }
}