namespace Inventory.Application.TransactionTypes.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdTransactionTypeQuery(long Id) : ICommand<TransactionTypeDto> , IGetByIdQuery<Result<TransactionTypeDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Inventory.Domain.TransactionType> _Repository, IMapper mapper) : GetCommandHandler<GetByIdTransactionTypeQuery, Inventory.Domain.TransactionType, TransactionTypeDto>(_Repository, mapper)
    {

        public override Expression<Func<Inventory.Domain.TransactionType, bool>> CreateFilter(GetByIdTransactionTypeQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}