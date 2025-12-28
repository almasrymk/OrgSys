namespace Application.Commands.Org.Setting.Bank.Queries
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

    public sealed record GetByIdBankQuery(long Id) : ICommand<BankModelView> , IGetByIdQuery<Result<BankModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Entity.Model.Bank> _Repository, IMapper mapper) : GetCommandHandler<GetByIdBankQuery, Entity.Model.Bank, BankModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Bank, bool>> CreateFilter(GetByIdBankQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Utility.Status.Deleted && e.Hide != true;
        }
    }
}