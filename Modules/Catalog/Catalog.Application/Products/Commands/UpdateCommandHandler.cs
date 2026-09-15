namespace Catalog.Application.Products.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Threading.Tasks;

    public sealed class UpdateProductCommand : ProductDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Catalog.Domain.Product> _Repository, IRepository<Catalog.Domain.ProductUnit> _ProductUnitRepository, IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateProductCommand, Catalog.Domain.Product>(_UnitOfWork, _Repository, mapper , _provider)
    {
        override public async Task<bool> SaveDetials(UpdateProductCommand request)
        {
            #region UpdateProduct
            var productUnits = request.ProductUnits ?? [];
            var ids = productUnits.Where(e => e.Id > 0).Select(e => e.Id).ToList();
            var removeList = await _ProductUnitRepository.GetListByFilterAsync(e => e.ProductId == request.Id && !ids.Contains(e.Id));

            var res = await RemoveDetails<ProductUnit>(removeList ?? []);
            if (!res) return false;
            var ob = mapper.Map<List<ProductUnit>>(productUnits);
            foreach (var productUnit in ob)
                productUnit.ProductId = request.Id;

            res = await UpdateDetails<ProductUnit>(ob);
            #endregion 

            return res;
        }
    }
}
