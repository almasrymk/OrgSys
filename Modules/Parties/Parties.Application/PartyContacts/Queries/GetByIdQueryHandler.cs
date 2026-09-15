namespace Parties.Application.PartyContacts.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdPartyContactQuery(long Id) : ICommand<PartyContactDto>, IGetByIdQuery<Result<PartyContactDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Parties.Domain.PartyContact> _Repository, IMapper mapper) : GetCommandHandler<GetByIdPartyContactQuery, Parties.Domain.PartyContact, PartyContactDto>(_Repository, mapper)
    {
        public override Expression<Func<Parties.Domain.PartyContact, bool>> CreateFilter(GetByIdPartyContactQuery request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
