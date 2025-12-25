namespace Application.Commands.Org.Setting.Currency.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record GetByIdCurrencyQuery(long Id) : ICommand<CurrencyModelView> , IGetByIdQuery<Result<CurrencyModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Entity.Model.Currency> _Repository, IMapper mapper) : GetCommandHandler<GetByIdCurrencyQuery, Entity.Model.Currency, CurrencyModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Currency, bool>> CreateFilter(GetByIdCurrencyQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Utility.Status.Deleted && e.Hide != true;
        }
    }
}