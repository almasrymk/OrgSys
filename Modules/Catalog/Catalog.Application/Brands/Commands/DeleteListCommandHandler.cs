namespace Catalog.Application.Brands.Commands
{
    using OrgSys.SharedKernel;
    using System.Linq.Expressions;

    public sealed record DeleteListBrandCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Catalog.Domain.Brand> _Repository, IServiceProvider provider) : DeleteCommandHandler<DeleteListBrandCommand, Catalog.Domain.Brand>(_UnitOfWork, _Repository, provider)
    {
        public override Expression<Func<Catalog.Domain.Brand, bool>> CreateFilter(DeleteListBrandCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
