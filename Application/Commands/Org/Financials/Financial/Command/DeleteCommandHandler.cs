namespace Application.Commands.Org.Financials.Financial.Commands
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

    public sealed record DeleteFinancialCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork,
        IRepository<Financial> _Repository,
        IRepository<Invoice> _RepositoryInvoice,
        IServiceProvider _provider) : DeleteCommandHandler<DeleteFinancialCommand, Financial>(_UnitOfWork, _Repository, _provider)
    {

        public override async Task<bool> RemoveDetails(DeleteFinancialCommand request)
        {

            var finanicial = await _Repository.GetByFilterAsync(e => e.Id == request.Id, "FinancialInvoices") ?? new Financial();

            foreach (var item in finanicial.FinancialInvoices)
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
            return e => e.Id == request.Id && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }

    }
}