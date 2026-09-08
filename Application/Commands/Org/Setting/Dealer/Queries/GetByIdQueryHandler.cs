namespace Application.Commands.Org.Setting.Dealer.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record GetByIdDealerQuery(long Id) : ICommand<DealerDto> , IGetByIdQuery<Result<DealerDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.Dealer> _Repository, IMapper mapper) : GetCommandHandler<GetByIdDealerQuery, Domain.Entities.Dealer, DealerDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Dealer, bool>> CreateFilter(GetByIdDealerQuery request)
        {
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }

        // See SearchQueryHandler.CreateInclude — this handler previously had no override at all, so
        // DealerGroupName/AccountName/CountryName/CityName/DistrictName all came back null on a
        // single-record fetch too.
        public override string CreateInclude()
        {
            return "DealerGroup,Account,Country,City,District";
        }
    }
}