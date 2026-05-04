namespace Application.Commands.Org.Setting.Unit.Queries
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

    public sealed record GetByIdUnitQuery(long Id) : ICommand<UnitModelView> , IGetByIdQuery<Result<UnitModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Entity.Model.Unit> _Repository, IMapper mapper) : GetCommandHandler<GetByIdUnitQuery, Entity.Model.Unit, UnitModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Unit, bool>> CreateFilter(GetByIdUnitQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Utility.Status.Deleted && e.Hide != true;
        }
    }
}