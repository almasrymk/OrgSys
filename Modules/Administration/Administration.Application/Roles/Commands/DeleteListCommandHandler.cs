namespace Administration.Application.Roles.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteListRoleCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Administration.Domain.Role> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListRoleCommand, Administration.Domain.Role>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Administration.Domain.Role, bool>> CreateFilter(DeleteListRoleCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}