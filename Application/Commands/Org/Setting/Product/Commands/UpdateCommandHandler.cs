namespace Application.Commands.Org.Setting.Product.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.Model;
    using Entity.ModelView;
    using System.Threading.Tasks;

    public sealed class UpdateProductCommand : ProductModelView, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Product> _Repository, IRepository<Entity.Model.ProductUnit> _ProductUnitRepository, IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateProductCommand, Entity.Model.Product>(_UnitOfWork, _Repository, mapper , _provider)
    {
        override public async Task<bool> SaveDetials(UpdateProductCommand request)
        {
            #region UpdateProduct
            var ids = request.ProductUnitList.Select(e => e.Id);
            var removeList = await _ProductUnitRepository.GetListByFilterAsync(e => e.ProductId == request.Id && !ids.Contains(e.Id));

            var res = await RemoveDetails<ProductUnit>(removeList!);
            if (!res) return false;
            var ob = mapper.Map<List<ProductUnit>>(request.ProductUnitList);
            res = await UpdateDetails<ProductUnit>(ob);
            #endregion 

            return res;
        }
    }
}