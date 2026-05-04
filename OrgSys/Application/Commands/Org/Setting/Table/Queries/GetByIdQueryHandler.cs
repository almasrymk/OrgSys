namespace Application.Commands.Org.Setting.Table.Queries
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

    public sealed record GetByIdTableQuery(long Id) : ICommand<TableModelView> , IGetByIdQuery<Result<TableModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Entity.Model.Table> _Repository, IMapper mapper) : GetCommandHandler<GetByIdTableQuery, Entity.Model.Table, TableModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Table, bool>> CreateFilter(GetByIdTableQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Utility.Status.Deleted && e.Hide != true;
        }
    }
}