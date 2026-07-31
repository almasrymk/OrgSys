namespace Application.Commands.Org.Setting.FinancialType.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record GetListFinancialTypeQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) :
        ICommandCollection<FinancialTypeDto>, IListQuery<ResultCollection<FinancialTypeDto>>;

    public sealed class GetListQueryHandler(IRepository<Domain.Entities.FinancialType> _Repository, IMapper mapper) : 
        ListCommandHandler<GetListFinancialTypeQuery, Domain.Entities.FinancialType, FinancialTypeDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.FinancialType, bool>> CreateFilter(GetListFinancialTypeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
           (string.IsNullOrEmpty(request.KeySearch) || ("" + e.Code).Contains(request.KeySearch)) &&
           (request.ParentId == 0 || e.ParentId == request.ParentId) &&
           (request.TypeId == 0 || e.TypeId == request.TypeId) &&
           e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Domain.Entities.FinancialType>, IOrderedQueryable<Domain.Entities.FinancialType>> CreateOrderBy(GetListFinancialTypeQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}