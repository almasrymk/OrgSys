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

    public sealed record GetListClassificationQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<ClassificationDto> , IListQuery<ResultCollection<ClassificationDto>>;

    public sealed class GetListQueryHandler(IRepository<Domain.Entities.Classification> _Repository, IMapper mapper) : ListCommandHandler<GetListClassificationQuery, Domain.Entities.Classification, ClassificationDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Classification, bool>> CreateFilter(GetListClassificationQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.Classification>, IOrderedQueryable<Domain.Entities.Classification>> CreateOrderBy(GetListClassificationQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}