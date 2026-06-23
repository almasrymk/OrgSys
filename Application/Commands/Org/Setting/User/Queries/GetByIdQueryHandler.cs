namespace Application.Commands.Org.Setting.User.Queries
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

    public sealed record GetByIdUserQuery(long Id) : ICommand<UserDto> , IGetByIdQuery<Result<UserDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.User> _Repository, IMapper mapper) : GetCommandHandler<GetByIdUserQuery, Domain.Entities.User, UserDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.User, bool>> CreateFilter(GetByIdUserQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "Role,Branch";
        }
    }
}