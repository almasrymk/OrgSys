namespace Parties.Application.PartyAddresses.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdPartyAddressQuery(long Id) : ICommand<PartyAddressDto>, IGetByIdQuery<Result<PartyAddressDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Parties.Domain.PartyAddress> _Repository, IMapper mapper) : GetCommandHandler<GetByIdPartyAddressQuery, Parties.Domain.PartyAddress, PartyAddressDto>(_Repository, mapper)
    {
        public override Expression<Func<Parties.Domain.PartyAddress, bool>> CreateFilter(GetByIdPartyAddressQuery request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
