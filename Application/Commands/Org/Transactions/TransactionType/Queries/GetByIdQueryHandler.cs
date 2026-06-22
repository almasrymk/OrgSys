namespace Application.Commands.Org.Transactions.TransactionType.Queries
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

    public sealed record GetByIdTransactionTypeQuery(long Id) : ICommand<TransactionTypeModelView> , IGetByIdQuery<Result<TransactionTypeModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.TransactionType> _Repository, IMapper mapper) : GetCommandHandler<GetByIdTransactionTypeQuery, Domain.Entities.TransactionType, TransactionTypeModelView>(_Repository, mapper)
    {

        public override Expression<Func<Domain.Entities.TransactionType, bool>> CreateFilter(GetByIdTransactionTypeQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}