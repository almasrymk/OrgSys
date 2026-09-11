namespace Organization.Application.Branches.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteListBranchCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Organization.Domain.Branch> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListBranchCommand, Organization.Domain.Branch>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Organization.Domain.Branch, bool>> CreateFilter(DeleteListBranchCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}