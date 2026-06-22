namespace Application.Commands.Org.Setting.Preference.Queries
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

    public sealed record GetListPreferenceQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<PreferenceModelView> , IListQuery<ResultCollection<PreferenceModelView>>;

    public sealed class GetListQueryHandler(IRepository<Domain.Entities.Preference> _Repository, IMapper mapper) : ListCommandHandler<GetListPreferenceQuery, Domain.Entities.Preference, PreferenceModelView>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Preference, bool>> CreateFilter(GetListPreferenceQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
             (string.IsNullOrEmpty(request.KeySearch) || e.Reference.Contains(request.KeySearch)) &&
            (request.ParentId == 0 || e.ParentId == request.ParentId) &&
            (request.TypeId == 0 || e.TypeId == request.TypeId) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.Preference>, IOrderedQueryable<Domain.Entities.Preference>> CreateOrderBy(GetListPreferenceQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}