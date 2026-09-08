namespace Application.Commands.Org.Setting.ReferenceType.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record GetListReferenceTypeQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<ReferenceTypeDto> , IListQuery<ResultCollection<ReferenceTypeDto>>;

    public sealed class GetListQueryHandler(IRepository<Domain.Entities.ReferenceType> _Repository, IMapper mapper) : ListCommandHandler<GetListReferenceTypeQuery, Domain.Entities.ReferenceType, ReferenceTypeDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.ReferenceType, bool>> CreateFilter(GetListReferenceTypeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Domain.Entities.ReferenceType>, IOrderedQueryable<Domain.Entities.ReferenceType>> CreateOrderBy(GetListReferenceTypeQuery request)
        {
            return q => q.OrderBy(e => e.Id);
        }
    }
}
