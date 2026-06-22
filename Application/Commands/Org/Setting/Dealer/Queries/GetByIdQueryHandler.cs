namespace Application.Commands.Org.Setting.Dealer.Queries
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

    public sealed record GetByIdDealerQuery(long Id) : ICommand<DealerModelView> , IGetByIdQuery<Result<DealerModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.Dealer> _Repository, IMapper mapper) : GetCommandHandler<GetByIdDealerQuery, Domain.Entities.Dealer, DealerModelView>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Dealer, bool>> CreateFilter(GetByIdDealerQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}