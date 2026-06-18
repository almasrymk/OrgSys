namespace Application.Commands.Org.Transactions.Transaction.Queries
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

    public sealed record GetByIdTransactionQuery(long Id) : ICommand<TransactionModelView> , IGetByIdQuery<Result<TransactionModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Entity.Model.Transaction> _Repository, IMapper mapper) : GetCommandHandler<GetByIdTransactionQuery, Entity.Model.Transaction, TransactionModelView>(_Repository, mapper)
    {
        public override string CreateInclude()
        {
            return "TransactionProducts,TransactionProducts.Product.ProductUnits.Unit,TransactionProducts.Product.ProductUnits";
        }

        public override Expression<Func<Entity.Model.Transaction, bool>> CreateFilter(GetByIdTransactionQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Utility.Status.Deleted && e.Hide != true;
        }
    }
}