namespace Organization.Application.Companies.Commands
{
    using OrgSys.SharedKernel;
    using System.Linq.Expressions;

    public sealed record DeleteListCompanyCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Organization.Domain.Company> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListCompanyCommand, Organization.Domain.Company>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Organization.Domain.Company, bool>> CreateFilter(DeleteListCompanyCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
