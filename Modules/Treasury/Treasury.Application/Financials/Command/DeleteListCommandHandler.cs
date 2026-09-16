namespace Treasury.Application.Financials.Commands
{
    using CommercialDocuments.Contracts.Invoices;
    using MediatR;
    using System.Linq.Expressions;

    public sealed record DeleteListFinancialCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork,
        IRepository<Treasury.Domain.Financial> _Repository,
        IRepository<Treasury.Domain.FinancialInvoice> _RepositoryFinancialInvoice,
        ISender sender,
        IServiceProvider _provider) : DeleteCommandHandler<DeleteListFinancialCommand, Treasury.Domain.Financial>(_UnitOfWork, _Repository , _provider)
    {
        public override async Task<bool> RemoveDetails(DeleteListFinancialCommand request)
        {
            var financials = await _Repository.GetListByFilterAsync( e => request.Ids.Contains(e.Id),"FinancialInvoices");
            var financialInvoices = financials!.SelectMany(e => e.FinancialInvoices);
            foreach (var item in financialInvoices)
                await sender.Send(new AdjustInvoiceSettlementCommand(item.InvoiceId ?? 0, -item.Amount));

            return await base.RemoveDetails(request);
        }
        public override Expression<Func<Treasury.Domain.Financial, bool>> CreateFilter(DeleteListFinancialCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
