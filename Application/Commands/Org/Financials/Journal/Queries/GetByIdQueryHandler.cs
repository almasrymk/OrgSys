namespace Application.Commands.Org.Financials.Journal.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record GetByIdJournalQuery(long Id) : ICommand<JournalDto> , IGetByIdQuery<Result<JournalDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.Journal> _Repository, IMapper mapper) :
        GetCommandHandler<GetByIdJournalQuery, Domain.Entities.Journal, JournalDto>(_Repository, mapper)
    {
        public override string CreateInclude()
        {
            return "JournalItems,JournalItems.Account";
        }

        public override Expression<Func<Domain.Entities.Journal, bool>> CreateFilter(GetByIdJournalQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}