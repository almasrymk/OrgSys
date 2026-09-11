namespace Organization.Application.Tables.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteTableCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Organization.Domain.Table> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteTableCommand, Organization.Domain.Table>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Organization.Domain.Table, bool>> CreateFilter(DeleteTableCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}