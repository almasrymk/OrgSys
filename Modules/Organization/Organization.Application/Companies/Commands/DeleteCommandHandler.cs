namespace Organization.Application.Companies.Commands
{
    using OrgSys.SharedKernel;
    using System.Linq.Expressions;

    public sealed record DeleteCompanyCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Organization.Domain.Company> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteCompanyCommand, Organization.Domain.Company>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Organization.Domain.Company, bool>> CreateFilter(DeleteCompanyCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
