namespace Application.Common.Queries
{
    using Application.Abstraction.Command;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using System.Linq.Expressions;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    public class SearchCommandHandler<TRequest, TModel, TResponse>(IRepository<TModel> _Repository, IMapper mapper) : ICommandPaginationHandler<TRequest, TResponse>
        where TRequest : ICommandPagination<TResponse>
        where TModel : Domain.Entities.BaseModel 
        where TResponse : Domain.Entities.BaseModel
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public virtual async Task<ResultPagination<TResponse>> Handle(TRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var res = await _Repository.GetPaginationByFilterAsync(CreateFilter(request) , CreateOrderBy(request), CreateInclude() , Page, PageSize);
                if (res != null && res.Items != null)
                {
                    return new ResultPagination<TResponse>(
                    HttpStatusCode.OK,
                    res.Items.Select(e=> mapper.Map<TResponse>(e)).ToList(),
                    res.Page, res.PageSize, res.TotalPages,
                    null);
                }

                return new ResultPagination<TResponse>(
                    HttpStatusCode.InternalServerError,
                    new List<TResponse>(), 0, 0, 0,
                    new List<Error> { new Error ( "Error" ) });
            }
            catch (Exception ex)
            {
                return new ResultPagination<TResponse>(
                    HttpStatusCode.InternalServerError,
                    new List<TResponse>(), 0, 0, 0,
                    new List<Error> { new Error(ex.Message) });
            }
        }

        public virtual Expression<Func<TModel, bool>> CreateFilter(TRequest request)
        {
            return e => true;
        }

        public virtual string CreateInclude()
        {
            return "";
        }

        public virtual Func<IQueryable<TModel>, IOrderedQueryable<TModel>> CreateOrderBy(TRequest request)
        {
            return null;
        }
    }
}