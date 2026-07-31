using Application.Interfaces.CQRS;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    public class CoreController<TGetById, TSearch, TList, TResponse>(ISender sender) : ControllerBase
        where TGetById : IGetByIdQuery<Result<TResponse>>
        where TSearch : ISearchQuery<ResultPagination<TResponse>>
        where TList : IListQuery<ResultCollection<TResponse>>
    {
        [HttpGet("GetById")]
        public virtual async Task<Result<TResponse>> GetById(long Id, CancellationToken cancellationToken)
        {
            var query = (TGetById)Activator.CreateInstance(typeof(TGetById), Id)!;
            return await sender.Send(query, cancellationToken);
        }

        [HttpGet("GetList")]
        public virtual async Task<ResultCollection<TResponse>> GetList(string? KeySearch, long ParentId, long TypeId, int Page, int PageSize, CancellationToken cancellationToken)
        {
            var query = (TList)Activator.CreateInstance(typeof(TList), KeySearch, ParentId, TypeId, Page, PageSize)!;
            return await sender.Send(query, cancellationToken);
        }

        [HttpGet("Search")]
        public virtual async Task<ResultPagination<TResponse>> Search(string? KeySearch, long ParentId, long TypeId, int Page, int PageSize, CancellationToken cancellationToken)
        {
            var query = (TSearch)Activator.CreateInstance(typeof(TSearch), KeySearch, ParentId, TypeId, Page, PageSize)!;
            return await sender.Send(query, cancellationToken);
        }
    }

    public class CoreController<TGetById, TSearch, TList, TCreate, TUpdate, TDelete, TDeleteList, TResponse>(ISender sender) : ControllerBase
        where TGetById : IGetByIdQuery<Result<TResponse>>
        where TSearch : ISearchQuery<ResultPagination<TResponse>>
        where TList : IListQuery<ResultCollection<TResponse>>
        where TCreate : ICreateCommand<Result>
        where TUpdate : IUpdateCommand<Result>
        where TDelete : IDeleteCommand<Result>
        where TDeleteList : IDeleteListCommand<Result>
    {
        [HttpGet("GetById")]
        public virtual async Task<Result<TResponse>> GetById(long Id, CancellationToken cancellationToken)
        {
            var query = (TGetById)Activator.CreateInstance(typeof(TGetById), Id)!;
            return await sender.Send(query, cancellationToken);
        }

        [HttpGet("GetList")]
        public virtual async Task<ResultCollection<TResponse>> GetList(string? KeySearch, long ParentId, long TypeId, int Page, int PageSize, CancellationToken cancellationToken)
        {
            var query = (TList)Activator.CreateInstance(typeof(TList), KeySearch, ParentId, TypeId, Page, PageSize)!;
            return await sender.Send(query, cancellationToken);
        }

        [HttpGet("Search")]
        public virtual async Task<ResultPagination<TResponse>> Search(string? KeySearch, long ParentId, long TypeId, int Page, int PageSize, CancellationToken cancellationToken)
        {
            var query = (TSearch)Activator.CreateInstance(typeof(TSearch), KeySearch, ParentId, TypeId, Page, PageSize)!;
            return await sender.Send(query, cancellationToken);
        }

        [HttpPost("Create")]
        public virtual async Task<Result> Create([FromBody] TCreate Create, CancellationToken cancellationToken)
        {
            return await sender.Send(Create, cancellationToken);
        }

        [HttpPut("Update")]
        public virtual async Task<Result> Update([FromBody] TUpdate Update, CancellationToken cancellationToken)
        {
            return await sender.Send(Update, cancellationToken);
        }

        [HttpDelete("Delete")]
        public virtual async Task<Result> Delete(long Id, CancellationToken cancellationToken)
        {
            var command = (TDelete)Activator.CreateInstance(typeof(TDelete), Id)!;
            return await sender.Send(command, cancellationToken);
        }

        [HttpDelete("DeleteList")]
        public virtual async Task<Result> DeleteList([FromQuery] List<long> Ids, CancellationToken cancellationToken)
        {
            var command = (TDeleteList)Activator.CreateInstance(typeof(TDeleteList), Ids)!;
            return await sender.Send(command, cancellationToken);
        }
    }

    public class BaseController<TGetById, TSearch, TList, TCreate, TUpdate, TDelete, TDeleteList, TResponse>(ISender sender) : CoreController<TGetById, TSearch, TList, TCreate, TUpdate, TDelete, TDeleteList, TResponse>(sender)
        where TGetById : IGetByIdQuery<Result<TResponse>>
        where TSearch : ISearchQuery<ResultPagination<TResponse>>
        where TList : IListQuery<ResultCollection<TResponse>>
        where TCreate : ICreateCommand<Result>
        where TUpdate : IUpdateCommand<Result>
        where TDelete : IDeleteCommand<Result>
        where TDeleteList : IDeleteListCommand<Result>
    {

    }

    public class BaseController<TGetById, TSearch, TList, TCreate, TUpdate, TDelete, TDeleteList, IGetMax, TResponse>(ISender sender) : CoreController<TGetById, TSearch, TList, TCreate, TUpdate, TDelete, TDeleteList, TResponse>(sender)
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
        public virtual async Task<object> GetMax(long ParentId, long TypeId, CancellationToken cancellationToken)
        {
            var query = (IGetMax)Activator.CreateInstance(typeof(IGetMax), TypeId, ParentId)!;
            return await sender.Send(query, cancellationToken);
        }
    }
}