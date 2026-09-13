namespace Parties.Application.Dealers.Queries
{
    using Accounting.Contracts.Accounts;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using MediatR;
    using System.Linq.Expressions;

    public sealed record GetByIdDealerQuery(long Id) : ICommand<DealerDto> , IGetByIdQuery<Result<DealerDto>>;

    public sealed class GetByIdQueryHandler(
        IRepository<Parties.Domain.Dealer> _Repository,
        ISender sender,
        IMapper mapper) : GetCommandHandler<GetByIdDealerQuery, Parties.Domain.Dealer, DealerDto>(_Repository, mapper)
    {
        public override Expression<Func<Parties.Domain.Dealer, bool>> CreateFilter(GetByIdDealerQuery request)
        {
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        // See SearchQueryHandler.CreateInclude — this handler previously had no override at all, so
        // DealerGroupName/AccountName/CountryName/CityName/DistrictName all came back null on a
        // single-record fetch too. Account was removed from this list (see the GeneralLedger
        // migration report) — AccountCode/AccountName are patched in below instead.
        public override string CreateInclude()
        {
            return "DealerGroup,Country,City,District";
        }

        public override async Task<Result<DealerDto>> Handle(GetByIdDealerQuery request, CancellationToken cancellationToken)
        {
            var result = await base.Handle(request, cancellationToken);

            if (result.Response?.AccountId is > 0)
            {
                var account = (await sender.Send(new GetAccountQuery(result.Response.AccountId.Value), cancellationToken)).Response;
                result.Response.AccountCode = account?.Code;
                result.Response.AccountName = account?.Name;
            }

            return result;
        }
    }
}
