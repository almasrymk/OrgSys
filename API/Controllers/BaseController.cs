using Application.Commands.Org.City.Queries;
using Application.Interfaces.CQRS;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{  
    public class BaseController<TGetById, TSearch, TCreate, TUpdate, TDelete , TDeleteList , TResponse>(ISender sender) : ControllerBase
    where TGetById : IGetByIdQuery<Result<TResponse>>
    where TSearch : ISearchQuery<ResultPagination<TResponse>>
    where TCreate : ICreateCommand<Result>
    where TUpdate : IUpdateCommand<Result>
    where TDelete : IDeleteCommand<Result>
    where TDeleteList : IDeleteListCommand<Result>
    {
        [HttpGet("GetById")]
        public virtual async Task<IActionResult> GetById(long Id, CancellationToken cancellationToken)
        {
            var query = (TGetById)Activator.CreateInstance(typeof(TGetById), Id)!;
            var res = await sender.Send(query, cancellationToken);
            return res.StatusCode == HttpStatusCode.OK ? Ok(res) : BadRequest(res);
        }

        [HttpGet("GetList")]
        public virtual async Task<IActionResult> GetList(CancellationToken cancellationToken)
        {
            var query = (TSearch)Activator.CreateInstance(typeof(TSearch), "", 0, 0, 1, 100)!;
            var res = await sender.Send(query, cancellationToken);
            return res.StatusCode == HttpStatusCode.OK ? Ok(res) : BadRequest(res);
        }

        [HttpGet("Search")]
        public virtual async Task<IActionResult> Search(string KeySearch, int Page, int PageSize, CancellationToken cancellationToken)
        {
            var query = (TSearch)Activator.CreateInstance(typeof(TSearch), KeySearch, 0, 0, Page, PageSize)!;
            var res = await sender.Send(query, cancellationToken);
            return res.StatusCode == HttpStatusCode.OK ? Ok(res) : BadRequest(res);
        }

        [HttpPost("Create")]
        public virtual async Task<IActionResult> Create([FromBody] TCreate Create, CancellationToken cancellationToken)
        {
            var res = await sender.Send(Create, cancellationToken);
            return res.StatusCode == HttpStatusCode.OK ? Ok(res) : BadRequest(res);
        }

        [HttpPut("Update")]
        public virtual async Task<IActionResult> Update(TUpdate Update, CancellationToken cancellationToken)
        {
            var res = await sender.Send(Update, cancellationToken);
            return res.StatusCode == HttpStatusCode.OK ? Ok(res) : BadRequest(res);
        }

        [HttpDelete("Delete")]
        public virtual async Task<IActionResult> Delete(long Id, CancellationToken cancellationToken)
        {
            var command = (TDelete)Activator.CreateInstance(typeof(TDelete), Id)!;
            var res = await sender.Send(command, cancellationToken);
            return res.StatusCode == HttpStatusCode.OK ? Ok(res) : BadRequest(res);
        }


        [HttpDelete("DeleteList")]
        public virtual async Task<IActionResult> DeleteList(List<long> Ids, CancellationToken cancellationToken)
        {
            var command = (TDeleteList)Activator.CreateInstance(typeof(TDeleteList), Ids)!;
            var res = await sender.Send(command, cancellationToken);
            return res.StatusCode == HttpStatusCode.OK ? Ok(res) : BadRequest(res);
        }
    }
}
