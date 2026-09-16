namespace SaaS.Application.Subscriptions.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdSubscriptionQuery(long Id) : ICommand<SubscriptionDto>, IGetByIdQuery<Result<SubscriptionDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Subscription> _Repository, IMapper mapper) : GetCommandHandler<GetByIdSubscriptionQuery, Subscription, SubscriptionDto>(_Repository, mapper)
    {
        public override Expression<Func<Subscription, bool>> CreateFilter(GetByIdSubscriptionQuery request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
