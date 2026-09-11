namespace Inventory.Application.Products.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteProductCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Inventory.Domain.Product> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteProductCommand, Inventory.Domain.Product>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Inventory.Domain.Product, bool>> CreateFilter(DeleteProductCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        override public async Task<bool> RemoveDetails(DeleteProductCommand request)
        {
            #region UpdateProduct           
            var res = await RemoveDetails<ProductUnit>(e=>e.ProductId == request.Id);
            if (!res) return false;           
            #endregion 

            return res;
        }
    }
}