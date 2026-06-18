namespace Application.Commands.Org.Invoices.Invoice.Commands
{
    using Application.Abstraction.Command;
    using Application.Commands.Org.Setting.Product.Commands;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.Model;
    using Entity.ModelView;

    public sealed class UpdateInvoiceCommand : Entity.ModelView.InvoiceModelView , ICommand, IUpdateCommand<Result>;
    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, 
        IRepository<Entity.Model.Invoice> _Repository ,
         IRepository<Entity.Model.InvoiceProduct> _InvoiceProductRepository,
        IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateInvoiceCommand, Entity.Model.Invoice>(_UnitOfWork, _Repository , mapper , _provider)
    {


        override public async Task<bool> SaveDetials(UpdateInvoiceCommand request)
        {
            #region UpdateProduct
            var ids = request.InvoiceProductList.Select(e => e.Id);
            var removeList = await _InvoiceProductRepository.GetListByFilterAsync(e => e.InvoiceId == request.Id && !ids.Contains(e.Id));

            var res = await RemoveDetails<InvoiceProduct>(removeList!);
            if (!res) return false;
            var ob = mapper.Map<List<InvoiceProduct>>(request.InvoiceProductList);
            res = await UpdateDetails<InvoiceProduct>(ob);
            #endregion 

            return res;
        }
    }
}