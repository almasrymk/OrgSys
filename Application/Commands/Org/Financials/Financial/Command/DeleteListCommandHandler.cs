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

    public sealed record DeleteListFinancialCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, 
        IRepository<Domain.Entities.Financial> _Repository, 
        IRepository<Domain.Entities.FinancialInvoice> _RepositoryFinancialInvoice, 
        IRepository<Domain.Entities.Invoice> _RepositoryInvoice, 
        IServiceProvider _provider) : DeleteCommandHandler<DeleteListFinancialCommand, Domain.Entities.Financial>(_UnitOfWork, _Repository , _provider)
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
        public override Expression<Func<Domain.Entities.Financial, bool>> CreateFilter(DeleteListFinancialCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }

    }
}