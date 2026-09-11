namespace Sales.Application.Invoices.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using global::Application.Commands.Org.Financials.Integration.JournalInvoice;

    public sealed class UpdateInvoiceCommand : InvoiceDto , ICommand, IUpdateCommand<Result>;
    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, 
        IRepository<Sales.Domain.Invoice> _Repository ,
         IRepository<Sales.Domain.InvoiceProduct> _InvoiceProductRepository,
        IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateInvoiceCommand, Sales.Domain.Invoice>(_UnitOfWork, _Repository , mapper , _provider)
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
