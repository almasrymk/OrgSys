namespace Organization.Application.Tables.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdTableQuery(long Id) : ICommand<TableDto> , IGetByIdQuery<Result<TableDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Organization.Domain.Table> _Repository, IMapper mapper) : GetCommandHandler<GetByIdTableQuery, Organization.Domain.Table, TableDto>(_Repository, mapper)
    {
        public override Expression<Func<Organization.Domain.Table, bool>> CreateFilter(GetByIdTableQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}