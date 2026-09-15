namespace Catalog.Application.Products.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteListProductCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Catalog.Domain.Product> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListProductCommand, Catalog.Domain.Product>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Catalog.Domain.Product, bool>> CreateFilter(DeleteListProductCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override async Task<bool> RemoveDetails(DeleteListProductCommand request)
        {
            #region UpdateProduct           
            var res = await RemoveDetails<ProductUnit>(e => request.Ids.Contains(e.ProductId));
            if (!res) return false;
            #endregion

            return res;
        }
    }
}