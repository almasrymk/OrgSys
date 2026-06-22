namespace Application.Commands.Org.Setting.AccountType.Queries
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

    public sealed record GetByIdAccountTypeQuery(long Id) : ICommand<AccountTypeModelView> , IGetByIdQuery<Result<AccountTypeModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.AccountType> _Repository, IMapper mapper) : GetCommandHandler<GetByIdAccountTypeQuery, Domain.Entities.AccountType, AccountTypeModelView>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.AccountType, bool>> CreateFilter(GetByIdAccountTypeQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}