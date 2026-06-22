namespace Application.Commands.Org.Setting.Bank.Queries
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

    public sealed record GetByIdBankQuery(long Id) : ICommand<BankModelView> , IGetByIdQuery<Result<BankModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.Bank> _Repository, IMapper mapper) : GetCommandHandler<GetByIdBankQuery, Domain.Entities.Bank, BankModelView>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Bank, bool>> CreateFilter(GetByIdBankQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}