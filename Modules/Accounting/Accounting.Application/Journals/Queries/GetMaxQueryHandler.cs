namespace Accounting.Application.Journals.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using System;
    using System.Linq.Expressions;

    public sealed record GetMaxJournalQuery(long TypeId , long ParentId) : ICommandOb<object> , IGetMaxQuery<object>;

    public sealed class GetMaxQueryHandler(IRepository<Accounting.Domain.Journal> _Repository) : GetMaxCommandHandler<GetMaxJournalQuery, Accounting.Domain.Journal>(_Repository)
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