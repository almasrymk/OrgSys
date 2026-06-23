namespace Application.Commands.Org.Setting.DealerGroup.Queries
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

    public sealed record GetByIdDealerGroupQuery(long Id) : ICommand<DealerGroupDto> , IGetByIdQuery<Result<DealerGroupDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.DealerGroup> _Repository, IMapper mapper) : GetCommandHandler<GetByIdDealerGroupQuery, Domain.Entities.DealerGroup, DealerGroupDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.DealerGroup, bool>> CreateFilter(GetByIdDealerGroupQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}