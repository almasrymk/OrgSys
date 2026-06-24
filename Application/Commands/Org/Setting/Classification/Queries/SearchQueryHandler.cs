namespace Application.Commands.Org.Setting.Classification.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;
    using Utility;

    public sealed record SearchClassificationQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<ClassificationDto> ,ISearchQuery<ResultPagination<ClassificationDto>>;

    public sealed class SearchQueryHandler(IRepository<Domain.Entities.Classification> _Repository, IMapper mapper) : SearchCommandHandler<SearchClassificationQuery, Domain.Entities.Classification, ClassificationDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Classification, bool>> CreateFilter(SearchClassificationQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.Classification>, IOrderedQueryable<Domain.Entities.Classification>> CreateOrderBy(SearchClassificationQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}