namespace Application.Commands.Org.Setting.Safe.Queries
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

    public sealed record GetByIdSafeQuery(long Id) : ICommand<SafeModelView> , IGetByIdQuery<Result<SafeModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.Safe> _Repository, IMapper mapper) : GetCommandHandler<GetByIdSafeQuery, Domain.Entities.Safe, SafeModelView>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Safe, bool>> CreateFilter(GetByIdSafeQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}