namespace Application.Commands.Org.Financials.Journal.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record GetByIdJournalQuery(long Id) : ICommand<JournalModelView> , IGetByIdQuery<Result<JournalModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Entity.Model.Journal> _Repository, IMapper mapper) :
        GetCommandHandler<GetByIdJournalQuery, Entity.Model.Journal, JournalModelView>(_Repository, mapper)
    {
        public override string CreateInclude()
        {
            return "JournalItems,JournalItems.Account";
        }

        public override Expression<Func<Entity.Model.Journal, bool>> CreateFilter(GetByIdJournalQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Utility.Status.Deleted && e.Hide != true;
        }
    }
}