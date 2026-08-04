namespace Application.Commands.Org.Invoices.Invoice.Commands
{
    using Application.Abstraction.Command;
    using Application.Commands.Org.Setting.Product.Commands;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Domain.Entities;
    using Application.DTOs;
    using Application.Commands.Org.Financials.Integration.JournalInvoice;

    public sealed class UpdateInvoiceCommand : Application.DTOs.InvoiceDto , ICommand, IUpdateCommand<Result>;
    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, 
        IRepository<Domain.Entities.Invoice> _Repository ,
         IRepository<Domain.Entities.InvoiceProduct> _InvoiceProductRepository,
        IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateInvoiceCommand, Domain.Entities.Invoice>(_UnitOfWork, _Repository , mapper , _provider)
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

            if (res)
            {
                await new InvoiceJournalIntegration(_provider).SyncAsync(request);
                res = await _Repository.UpdateAsync(request);
            }

            return res;
        }
    }
}
