namespace Application.Commands.Org.Setting.User.Queries
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

    public sealed record GetByIdUserQuery(long Id) : ICommand<UserModelView> , IGetByIdQuery<Result<UserModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Entity.Model.User> _Repository, IMapper mapper) : GetCommandHandler<GetByIdUserQuery, Entity.Model.User, UserModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.User, bool>> CreateFilter(GetByIdUserQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Utility.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "Role,Branch";
        }
    }
}