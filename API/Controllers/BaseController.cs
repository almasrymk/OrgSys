using Application.Interfaces.CQRS;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    public class CoreController<TGetById, TSearch , TList , TCreate, TUpdate, TDelete, TDeleteList, TResponse>(ISender sender) : ControllerBase
        where TGetById : IGetByIdQuery<Result<TResponse>>
        where TSearch : ISearchQuery<ResultPagination<TResponse>>
        where TList : IListQuery<ResultCollection<TResponse>>
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
        public virtual async Task<IActionResult> GetList(string? KeySearch, long ParentId, long TypeId, int Page, int PageSize, CancellationToken cancellationToken)
        {
            var query = (TList)Activator.CreateInstance(typeof(TList), KeySearch, ParentId, TypeId, Page, PageSize)!;
            var res = await sender.Send(query, cancellationToken);
            return res.StatusCode == HttpStatusCode.OK ? Ok(res) : BadRequest(res);
        }

        [HttpGet("Search")]
        public virtual async Task<IActionResult> Search(string? KeySearch, long ParentId, long TypeId, int Page, int PageSize, CancellationToken cancellationToken)
        {
            var query = (TSearch)Activator.CreateInstance(typeof(TSearch), KeySearch, ParentId, TypeId, Page, PageSize)!;
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
        public virtual async Task<IActionResult> Update([FromBody] TUpdate Update, CancellationToken cancellationToken)
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
        public virtual async Task<IActionResult> DeleteList([FromQuery] List<long> Ids, CancellationToken cancellationToken)
        {
            var command = (TDeleteList)Activator.CreateInstance(typeof(TDeleteList), Ids)!;
            var res = await sender.Send(command, cancellationToken);
            return res.StatusCode == HttpStatusCode.OK ? Ok(res) : BadRequest(res);
        }
    }

    public class BaseController<TGetById, TSearch , TList , TCreate, TUpdate, TDelete, TDeleteList, TResponse>(ISender sender) : CoreController<TGetById, TSearch , TList , TCreate, TUpdate, TDelete, TDeleteList, TResponse>(sender)
        where TGetById : IGetByIdQuery<Result<TResponse>>
        where TSearch : ISearchQuery<ResultPagination<TResponse>>
        where TList : IListQuery<ResultCollection<TResponse>>
        where TCreate : ICreateCommand<Result>
        where TUpdate : IUpdateCommand<Result>
        where TDelete : IDeleteCommand<Result>
        where TDeleteList : IDeleteListCommand<Result>
    {

    }

    public class BaseController<TGetById, TSearch , TList , TCreate, TUpdate, TDelete, TDeleteList, IGetMax, TResponse>(ISender sender) : CoreController<TGetById, TSearch , TList , TCreate, TUpdate, TDelete, TDeleteList, TResponse>(sender)
        where TGetById : IGetByIdQuery<Result<TResponse>>
        where TSearch : ISearchQuery<ResultPagination<TResponse>>
        where TList : IListQuery<ResultCollection<TResponse>>
        where TCreate : ICreateCommand<Result>
        where TUpdate : IUpdateCommand<Result>
        where TDelete : IDeleteCommand<Result>
        where IGetMax : IGetMaxQuery<object>
        where TDeleteList : IDeleteListCommand<Result>
    {

        [HttpGet("GetMax")]
        public virtual async Task<IActionResult> GetMax(long ParentId, long TypeId, CancellationToken cancellationToken)
        {
            var query = (IGetMax)Activator.CreateInstance(typeof(IGetMax), TypeId , ParentId)!;
            var res = await sender.Send(query, cancellationToken);
            return Ok(res);
        }        
    }
}