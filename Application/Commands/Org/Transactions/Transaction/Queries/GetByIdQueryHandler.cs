namespace Application.Commands.Org.Transactions.Transaction.Queries
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

    public sealed record GetByIdTransactionQuery(long Id) : ICommand<TransactionModelView> , IGetByIdQuery<Result<TransactionModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.Transaction> _Repository, IMapper mapper) : GetCommandHandler<GetByIdTransactionQuery, Domain.Entities.Transaction, TransactionModelView>(_Repository, mapper)
    {
        public override string CreateInclude()
        {
            return "TransactionProducts,TransactionProducts.Product.ProductUnits.Unit,TransactionProducts.Product.ProductUnits";
        }

        public override Expression<Func<Domain.Entities.Transaction, bool>> CreateFilter(GetByIdTransactionQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}