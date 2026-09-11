namespace Organization.Application.Branches.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteBranchCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Organization.Domain.Branch> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteBranchCommand, Organization.Domain.Branch>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Organization.Domain.Branch, bool>> CreateFilter(DeleteBranchCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}