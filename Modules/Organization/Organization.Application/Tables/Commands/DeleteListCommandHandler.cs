namespace Organization.Application.Tables.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteListTableCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Organization.Domain.Table> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListTableCommand, Organization.Domain.Table>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Organization.Domain.Table, bool>> CreateFilter(DeleteListTableCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}