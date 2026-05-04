namespace Application.Commands.Org.Setting.Account.Queries
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

    public sealed record GetByIdAccountQuery(long Id) : ICommand<AccountModelView> , IGetByIdQuery<Result<AccountModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Entity.Model.Account> _Repository, IMapper mapper) : GetCommandHandler<GetByIdAccountQuery, Entity.Model.Account, AccountModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Account, bool>> CreateFilter(GetByIdAccountQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Utility.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "AccountType";
        }
    }
}