namespace Application.Commands.Org.Setting.Preference.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using Domain.Abstraction;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record GetListPreferenceQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<PreferenceDto> , IListQuery<ResultCollection<PreferenceDto>>;

    public sealed class GetListQueryHandler(IRepository<Domain.Entities.Preference> _Repository, IMapper mapper) : ListCommandHandler<GetListPreferenceQuery, Domain.Entities.Preference, PreferenceDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Preference, bool>> CreateFilter(GetListPreferenceQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
             (string.IsNullOrEmpty(request.KeySearch) || e.Reference.Contains(request.KeySearch)) &&
            (request.ParentId == 0 || e.ParentId == request.ParentId) &&
            (request.TypeId == 0 || e.TypeId == request.TypeId) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.Preference>, IOrderedQueryable<Domain.Entities.Preference>> CreateOrderBy(GetListPreferenceQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}