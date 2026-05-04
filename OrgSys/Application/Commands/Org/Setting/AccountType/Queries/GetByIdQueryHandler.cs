namespace Application.Commands.Org.Setting.AccountType.Queries
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

    public sealed record GetByIdAccountTypeQuery(long Id) : ICommand<AccountTypeModelView> , IGetByIdQuery<Result<AccountTypeModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Entity.Model.AccountType> _Repository, IMapper mapper) : GetCommandHandler<GetByIdAccountTypeQuery, Entity.Model.AccountType, AccountTypeModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.AccountType, bool>> CreateFilter(GetByIdAccountTypeQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Utility.Status.Deleted && e.Hide != true;
        }
    }
}