namespace Application.Commands.Org.Financials.Journal.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using Domain.Abstraction;
    using Entity.Model;
    using System;
    using System.Linq.Expressions;

    public sealed record GetMaxJournalQuery(long TypeId , long ParentId) : ICommandOb<object> , IGetMaxQuery<object>;

    public sealed class GetMaxQueryHandler(IRepository<Entity.Model.Journal> _Repository) : GetMaxCommandHandler<GetMaxJournalQuery, Entity.Model.Journal>(_Repository)
    {
        public override Expression<Func<Journal, bool>> CreateFilter(GetMaxJournalQuery request)
        {
            return e=>e.TypeId == request.TypeId;
        }

        public override Expression<Func<Journal, object>> CreateSelector()
        {
            return e => e.CodeNumber;
        }
    }
}