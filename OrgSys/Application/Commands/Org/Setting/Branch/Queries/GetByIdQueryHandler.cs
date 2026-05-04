namespace Application.Commands.Org.Setting.Branch.Queries
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

    public sealed record GetByIdBranchQuery(long Id) : ICommand<BranchModelView> , IGetByIdQuery<Result<BranchModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Entity.Model.Branch> _Repository, IMapper mapper) : GetCommandHandler<GetByIdBranchQuery, Entity.Model.Branch, BranchModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Branch, bool>> CreateFilter(GetByIdBranchQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Utility.Status.Deleted && e.Hide != true;
        }
    }
}