namespace Administration.Application.Roles.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteRoleCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Administration.Domain.Role> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteRoleCommand, Administration.Domain.Role>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Administration.Domain.Role, bool>> CreateFilter(DeleteRoleCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}