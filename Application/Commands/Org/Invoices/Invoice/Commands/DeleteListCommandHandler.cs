namespace Application.Commands.Org.Invoices.Invoice.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.Model;
    using Entity.ModelView;
    using Microsoft.Extensions.DependencyInjection;
    using System.Linq.Expressions;

    public sealed record DeleteListInvoiceCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Invoice> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListInvoiceCommand, Entity.Model.Invoice>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Entity.Model.Invoice, bool>> CreateFilter(DeleteListInvoiceCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Utility.Status.Deleted && e.Hide != true;
        }


        public override async Task<bool> RemoveDetails(DeleteListInvoiceCommand request)
        {
            var financialRepo = _provider.GetRequiredService<IRepository<Financial>>();
            var transactionRepo = _provider.GetRequiredService<IRepository<Transaction>>();

            foreach (var invoiceId in request.Ids)
            {
                var invoice = await _Repository.GetByFilterAsync(i => i.Id == invoiceId,"InvoiceProducts");

                if (invoice == null)
                    continue;

                invoice.InvoiceProducts.Clear();

                var financial = await financialRepo
                    .GetByFilterAsync(e => e.FinancialInvoices.Select(f => f.InvoiceId)
                    .Contains(invoiceId),"FinancialInvoices");

                if (financial != null)
                    await financialRepo.ShiftDeleteAsync(f => f.Id == financial.Id);
                
                var transaction = await transactionRepo
                    .GetByFilterAsync(e => e.Id == invoice.TransactionId,"TransactionProducts");

                if (transaction != null)
                    await transactionRepo.ShiftDeleteAsync(t => t.Id == transaction.Id);
            }

            return await _UnitOfWork.SaveChangeAsync() > 0;
        }
    }
}