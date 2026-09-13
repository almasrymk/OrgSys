using MasterData.Application.Districts.Commands;
using MasterData.Application.Districts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class DistrictController(ISender sender) : BaseController<GetByIdDistrictQuery, SearchDistrictQuery , GetListDistrictQuery, CreateDistrictCommand, UpdateDistrictCommand, DeleteDistrictCommand, DeleteListDistrictCommand, DistrictDto>(sender)
    {
        [HttpGet("GetListByCityId")]
        public virtual async Task<IActionResult> GetListByCityId(string? KeySearch , long CityId , long ParentId, long TypeId, int Page, int PageSize, CancellationToken cancellationToken)
        {
            var query = (GetListByCityDistrictQuery)Activator.CreateInstance(typeof(GetListByCityDistrictQuery), KeySearch , CityId , ParentId, TypeId, Page, PageSize)!;
            var res = await Sender.Send(query, cancellationToken);
            return res.StatusCode == HttpStatusCode.OK ? Ok(res) : BadRequest(res);
        }        
    }
}