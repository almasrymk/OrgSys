namespace Treasury.Application.Financials.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using Microsoft.Extensions.DependencyInjection;
    using System.Linq.Expressions;

    public sealed record DeleteListFinancialCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, 
        IRepository<Treasury.Domain.Financial> _Repository, 
        IRepository<Treasury.Domain.FinancialInvoice> _RepositoryFinancialInvoice, 
        IRepository<Sales.Domain.Invoice> _RepositoryInvoice, 
        IServiceProvider _provider) : DeleteCommandHandler<DeleteListFinancialCommand, Treasury.Domain.Financial>(_UnitOfWork, _Repository , _provider)
    {

        public override async Task<bool> RemoveDetails(DeleteListFinancialCommand request)
        {

            var financials = await _Repository.GetListByFilterAsync( e => request.Ids.Contains(e.Id),"FinancialInvoices");

            var financialInvoices = financials!.SelectMany(e => e.FinancialInvoices);
            foreach (var item in financialInvoices)
            {

                var invoice = await _RepositoryInvoice.GetByFilterAsync(e => e.Id == item.InvoiceId, "") ?? new() ;
                invoice.Credit += item.Amount;
                invoice.Paid -= item.Amount;
                await _RepositoryInvoice.UpdateAsync(invoice);
            }
            //finanicial.FinancialInvoices.Clear();

            return await base.RemoveDetails(request);
        }
        public override Expression<Func<Treasury.Domain.Financial, bool>> CreateFilter(DeleteListFinancialCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

    }
}