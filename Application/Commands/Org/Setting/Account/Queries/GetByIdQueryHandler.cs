namespace Application.Commands.Org.Setting.Account.Queries
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

    public sealed record GetByIdAccountQuery(long Id) : ICommand<AccountModelView> , IGetByIdQuery<Result<AccountModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.Account> _Repository, IMapper mapper) : GetCommandHandler<GetByIdAccountQuery, Domain.Entities.Account, AccountModelView>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Account, bool>> CreateFilter(GetByIdAccountQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "AccountType";
        }
    }
}