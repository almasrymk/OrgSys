namespace Catalog.Application.Brands.Commands
{
    using OrgSys.SharedKernel;
    using System.Linq.Expressions;

    public sealed record DeleteBrandCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Catalog.Domain.Brand> _Repository, IServiceProvider provider) : DeleteCommandHandler<DeleteBrandCommand, Catalog.Domain.Brand>(_UnitOfWork, _Repository, provider)
    {
        public override Expression<Func<Catalog.Domain.Brand, bool>> CreateFilter(DeleteBrandCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
