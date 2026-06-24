namespace Application.Commands.Org.Setting.Currency.Queries
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

    public sealed record GetByIdCurrencyQuery(long Id) : ICommand<CurrencyDto> , IGetByIdQuery<Result<CurrencyDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.Currency> _Repository, IMapper mapper) : GetCommandHandler<GetByIdCurrencyQuery, Domain.Entities.Currency, CurrencyDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Currency, bool>> CreateFilter(GetByIdCurrencyQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}