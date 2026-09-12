namespace Parties.Application.DealerGroups.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteListDealerGroupCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Parties.Domain.DealerGroup> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListDealerGroupCommand, Parties.Domain.DealerGroup>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Parties.Domain.DealerGroup, bool>> CreateFilter(DeleteListDealerGroupCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}