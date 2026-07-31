namespace Application.Commands.Org.Setting.JournalType.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Common.Queries;
    using Application.DTOs;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using System.Linq.Expressions;

    public sealed record GetByIdJournalTypeQuery(long Id) : ICommand<JournalTypeDto>, IGetByIdQuery<Result<JournalTypeDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.JournalType> repository, IMapper mapper)
        : GetCommandHandler<GetByIdJournalTypeQuery, Domain.Entities.JournalType, JournalTypeDto>(repository, mapper)
    {
        public override Expression<Func<Domain.Entities.JournalType, bool>> CreateFilter(GetByIdJournalTypeQuery request)
        {
            return e => e.Id == request.Id && e.Status != Domain.Enums.Status.Deleted;
        }
    }
}
