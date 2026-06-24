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

          var transactionProducts =  await _Repository.GetByFilterAsync(t => t.Id == request.Id, "TransactionProducts");

            if(transactionProducts != null && transactionProducts.TransactionProducts.Count > 0)
            {
                transactionProducts.TransactionProducts.Clear();
                return true;
            }

            return false;
        }
    }
}