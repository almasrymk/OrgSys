namespace Inventory.Application.Properties.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdPropertyQuery(long Id) : ICommand<PropertyDto> , IGetByIdQuery<Result<PropertyDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Inventory.Domain.Property> _Repository, IMapper mapper) : GetCommandHandler<GetByIdPropertyQuery, Inventory.Domain.Property, PropertyDto>(_Repository, mapper)
    {
        public override Expression<Func<Inventory.Domain.Property, bool>> CreateFilter(GetByIdPropertyQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}