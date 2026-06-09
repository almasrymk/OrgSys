namespace Application.Commands.Org.Transactions.TransactionType.Queries
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

    public sealed record GetByIdTransactionTypeQuery(long Id) : ICommand<TransactionTypeModelView> , IGetByIdQuery<Result<TransactionTypeModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Entity.Model.TransactionType> _Repository, IMapper mapper) : GetCommandHandler<GetByIdTransactionTypeQuery, Entity.Model.TransactionType, TransactionTypeModelView>(_Repository, mapper)
    {

        public override Expression<Func<Entity.Model.TransactionType, bool>> CreateFilter(GetByIdTransactionTypeQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Utility.Status.Deleted && e.Hide != true;
        }
    }
}