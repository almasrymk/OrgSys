namespace Application.Commands.Org.Invoices.Invoice.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Domain.Entities;
    using Application.DTOs;
    using Microsoft.Extensions.DependencyInjection;
    using System.Linq.Expressions;
    using Domain.Enums;
    using Application.Commands.Org.Financials.Integration.JournalInvoice;

    public sealed record DeleteInvoiceCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork,
        IRepository<Invoice> _Repository,
        IRepository<Financial> _FinancialRepo,
        IServiceProvider _provider) : DeleteCommandHandler<DeleteInvoiceCommand,Invoice>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Domain.Entities.Invoice, bool>> CreateFilter(DeleteInvoiceCommand request)
        {
            return e => e.Id == request.Id && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }

        public override async Task<bool> RemoveDetails(DeleteInvoiceCommand request)
        {
            var invoice = await _Repository.GetByFilterAsync(i => i.Id == request.Id, "InvoiceProducts");
            if (invoice == null)
                return false;

            await new InvoiceJournalIntegration(_provider).DeleteByInvoiceIdAsync(request.Id);
            invoice.InvoiceProducts!.Clear();

            //var financialRepo = _provider.GetRequiredService<IRepository<Financial>>();

            var financial = await _FinancialRepo.GetByFilterAsync(e => e.FinancialInvoices.Select(f => f.InvoiceId).Contains(request.Id), "FinancialInvoices");
            if (financial != null)
            {
                //financial.FinancialInvoices.Clear();
                await _FinancialRepo.ShiftDeleteAsync(f => f.Id == financial.Id);

            }

            var transactionRepo = _provider.GetRequiredService<IRepository<Transaction>>();

            var transaction = await transactionRepo.GetByFilterAsync(e => e.Id == invoice.TransactionId, "TransactionProducts");

            if (transaction != null)
            {
                //transaction.TransactionProducts.Clear();
                await transactionRepo.ShiftDeleteAsync(t => t.Id == transaction.Id);
            }

            return await _UnitOfWork.SaveChangeAsync() > 0;
        }
    }
}