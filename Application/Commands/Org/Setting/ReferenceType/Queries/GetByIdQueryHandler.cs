namespace Application.Commands.Org.Setting.ReferenceType.Queries
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

    public sealed record GetByIdReferenceTypeQuery(long Id) : ICommand<ReferenceTypeDto> , IGetByIdQuery<Result<ReferenceTypeDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.ReferenceType> _Repository, IMapper mapper) : GetCommandHandler<GetByIdReferenceTypeQuery, Domain.Entities.ReferenceType, ReferenceTypeDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.ReferenceType, bool>> CreateFilter(GetByIdReferenceTypeQuery request)
        {
            return e => e.Id == request.Id && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}
