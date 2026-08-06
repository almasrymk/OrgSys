namespace Application.Commands.Org.Transactions.Transaction.Commands
{
    using Application.Abstraction.Command;
    using Application.Commands.Org.Invoices.Invoice.Commands;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Domain.Entities;
    using Application.DTOs;
    using Microsoft.Extensions.DependencyInjection;
    using System.Linq.Expressions;
    using Application.Commands.Org.Financials.Integration.JournalTransaction;

    public sealed record DeleteTransactionCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork,
        IRepository<Domain.Entities.Transaction> _Repository,
        IRepository<TransactionProduct> _TransactionProductRepository,
        IServiceProvider _provider) : DeleteCommandHandler<DeleteTransactionCommand, Domain.Entities.Transaction>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Domain.Entities.Transaction, bool>> CreateFilter(DeleteTransactionCommand request)
        {
            return e => e.Id == request.Id && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }

        public override async Task<bool> RemoveDetails(DeleteTransactionCommand request)
        {
            var sourceTransaction = await _Repository.GetByFilterAsync(e => e.Id == request.Id, string.Empty);
            if (sourceTransaction?.InventoryId is > 0)
                throw new InvalidOperationException("A transaction created from an inventory cannot be deleted");

            var sourceInvoice = await _provider.GetRequiredService<IRepository<Domain.Entities.Invoice>>()
                .GetByFilterAsync(e => e.TransactionId == request.Id, string.Empty);
            if (sourceInvoice != null)
                throw new InvalidOperationException("A transaction created from an invoice cannot be deleted");

          var transactionProducts =  await _Repository.GetByFilterAsync(t => t.Id == request.Id, "TransactionProducts");

            if (transactionProducts == null)
                return false;

            await new TransactionJournalIntegration(_provider).DeleteByTransactionIdAsync(request.Id);
            return await _TransactionProductRepository.ShiftDeleteAsync(e => e.TransactionId == request.Id);
        }
    }
}
