namespace Application.Commands.Org.Setting.Role.Queries
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

    public sealed record GetByIdRoleQuery(long Id) : ICommand<RoleModelView> , IGetByIdQuery<Result<RoleModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Entity.Model.Role> _Repository, IMapper mapper) : GetCommandHandler<GetByIdRoleQuery, Entity.Model.Role, RoleModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Role, bool>> CreateFilter(GetByIdRoleQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Utility.Status.Deleted && e.Hide != true;
        }
    }
}