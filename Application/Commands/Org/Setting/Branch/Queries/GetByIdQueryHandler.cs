namespace Application.Commands.Org.Setting.Branch.Queries
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

    public sealed record GetByIdBranchQuery(long Id) : ICommand<BranchModelView> , IGetByIdQuery<Result<BranchModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.Branch> _Repository, IMapper mapper) : GetCommandHandler<GetByIdBranchQuery, Domain.Entities.Branch, BranchModelView>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Branch, bool>> CreateFilter(GetByIdBranchQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}