namespace Application.Commands.Org.Setting.Shift.Queries
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

    public sealed record GetByIdShiftQuery(long Id) : ICommand<ShiftModelView> , IGetByIdQuery<Result<ShiftModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Entity.Model.Shift> _Repository, IMapper mapper) : GetCommandHandler<GetByIdShiftQuery, Entity.Model.Shift, ShiftModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Shift, bool>> CreateFilter(GetByIdShiftQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Utility.Status.Deleted && e.Hide != true;
        }
    }
}