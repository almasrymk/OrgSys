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

    public class ListCommandHandler<TRequest, TModel, TResponse>(IRepository<TModel> _Repository, IMapper mapper) : ICommandCollectionHandler<TRequest, TResponse>
        where TRequest : ICommandCollection<TResponse>
        where TModel : Entity.BaseModel 
        where TResponse : Entity.BaseModel
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public virtual async Task<ResultCollection<TResponse>> Handle(TRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var res = await _Repository.GetListByFilterAsync(CreateFilter(request) , CreateOrderBy(request) , CreateInclude(), Page, PageSize);
                if (res != null)
                {
                    return new ResultCollection<TResponse>(
                    HttpStatusCode.OK,
                    res.Select(e=> mapper.Map<TResponse>(e)).ToList(),
                    null);
                }

                return new ResultCollection<TResponse>(
                    HttpStatusCode.InternalServerError,
                    new List<TResponse>(),
                    new List<Error> { new Error("Error") });
            }
            catch (Exception ex)
            {
                return new ResultCollection<TResponse>(
                    HttpStatusCode.InternalServerError,
                    new List<TResponse>(),
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
            return e => e.OrderBy(s => s.Id);
        }
    }
}