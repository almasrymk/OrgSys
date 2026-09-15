namespace Catalog.Application.Attributes.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdPropertyQuery(long Id) : ICommand<PropertyDto> , IGetByIdQuery<Result<PropertyDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Catalog.Domain.Property> _Repository, IMapper mapper) : GetCommandHandler<GetByIdPropertyQuery, Catalog.Domain.Property, PropertyDto>(_Repository, mapper)
    {
        public override Expression<Func<Catalog.Domain.Property, bool>> CreateFilter(GetByIdPropertyQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}