namespace Application.Commands.Org.Setting.Outlay.Queries
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

    public sealed record GetByIdOutlayQuery(long Id) : ICommand<OutlayModelView> , IGetByIdQuery<Result<OutlayModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.Outlay> _Repository, IMapper mapper) : GetCommandHandler<GetByIdOutlayQuery, Domain.Entities.Outlay, OutlayModelView>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Outlay, bool>> CreateFilter(GetByIdOutlayQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}