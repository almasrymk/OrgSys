namespace Application.Commands.Org.Setting.Table.Queries
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

    public sealed record GetByIdTableQuery(long Id) : ICommand<TableModelView> , IGetByIdQuery<Result<TableModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.Table> _Repository, IMapper mapper) : GetCommandHandler<GetByIdTableQuery, Domain.Entities.Table, TableModelView>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Table, bool>> CreateFilter(GetByIdTableQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}