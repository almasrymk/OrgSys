namespace Treasury.Application.Financials.Commands
{
    using CommercialDocuments.Contracts.Invoices;
    using MediatR;
    using System.Linq.Expressions;

    public sealed record DeleteFinancialCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork,
        IRepository<Financial> _Repository,
        ISender sender,
        IServiceProvider _provider) : DeleteCommandHandler<DeleteFinancialCommand, Financial>(_UnitOfWork, _Repository, _provider)
    {
        public override async Task<bool> RemoveDetails(DeleteFinancialCommand request)
        {
            var finanicial = await _Repository.GetByFilterAsync(e => e.Id == request.Id, "FinancialInvoices") ?? new Financial();

            foreach (var item in finanicial.FinancialInvoices!)
                await sender.Send(new AdjustInvoiceSettlementCommand(item.InvoiceId ?? 0, -item.Amount));

            return await base.RemoveDetails(request);
        }
        public override Expression<Func<Financial, bool>> CreateFilter(DeleteFinancialCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true && e.Posted != true;
        }
    }
}
