namespace Application.Commands.Org.Setting.CashBox.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record GetByIdCashBoxQuery(long Id) : ICommand<CashBoxDto>, IGetByIdQuery<Result<CashBoxDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.CashBox> _Repository, IMapper mapper) : GetCommandHandler<GetByIdCashBoxQuery, Domain.Entities.CashBox, CashBoxDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.CashBox, bool>> CreateFilter(GetByIdCashBoxQuery request)
        {
            return e => e.Id == request.Id && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}
