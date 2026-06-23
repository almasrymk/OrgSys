namespace Application.Commands.Org.Setting.Stock.Queries
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

    public sealed record GetByIdStockQuery(long Id) : ICommand<StockDto> , IGetByIdQuery<Result<StockDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.Stock> _Repository, IMapper mapper) : GetCommandHandler<GetByIdStockQuery, Domain.Entities.Stock, StockDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Stock, bool>> CreateFilter(GetByIdStockQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}