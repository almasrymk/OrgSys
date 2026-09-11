namespace Treasury.Application.Financials.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using Microsoft.Extensions.DependencyInjection;
    using System.Linq.Expressions;

    public sealed record DeleteFinancialCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork,
        IRepository<Financial> _Repository,
        IRepository<Invoice> _RepositoryInvoice,
        IServiceProvider _provider) : DeleteCommandHandler<DeleteFinancialCommand, Financial>(_UnitOfWork, _Repository, _provider)
    {

        public override async Task<bool> RemoveDetails(DeleteFinancialCommand request)
        {

            var finanicial = await _Repository.GetByFilterAsync(e => e.Id == request.Id, "FinancialInvoices") ?? new Financial();

            foreach (var item in finanicial.FinancialInvoices!)
            {
                var invoice = await _RepositoryInvoice.GetByFilterAsync(e => e.Id == item.InvoiceId, "") ?? new Invoice();
                invoice.Credit += item.Amount;
                invoice.Paid -= item.Amount;
                await _RepositoryInvoice.UpdateAsync(invoice);
            }
             //finanicial.FinancialInvoices.Clear();

            return await base.RemoveDetails(request);
        }
        public override Expression<Func<Financial, bool>> CreateFilter(DeleteFinancialCommand request)
        {
            // A Posted financial transaction (e.g. a Customer Receipt) is immutable — it is
            // deliberately excluded here so it cannot be deleted; correct it via Reverse instead.
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true && e.Posted != true;
        }

    }
}