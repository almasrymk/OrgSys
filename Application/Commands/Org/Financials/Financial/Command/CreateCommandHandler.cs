namespace Application.Commands.Org.Financials.Financial.Commands;

using Application.Abstraction.Command;
using Application.Common.Commands;
using Application.Interfaces.CQRS;
using AutoMapper;
using Domain.Abstraction;
using Domain.Shared;
using Domain.Entities;
using Application.DTOs;
using System.Net;

public sealed class CreateFinancialCommand : Application.DTOs.FinancialModelView, ICommand , ICreateCommand<Result>;

public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, 
    IRepository<Domain.Entities.Financial> _Repository, 
    IRepository<Invoice> _RepositoryInvoice,
    IRepository<FinancialInvoice> _RepositoryFinancialInvoice,
    IMapper mapper) : CreateCommandHandler<CreateFinancialCommand, Domain.Entities.Financial>(_UnitOfWork, _Repository , mapper)
{

    public override async Task<Result> Handle(CreateFinancialCommand request, CancellationToken cancellationToken)
    {
        await _UnitOfWork.BeginTransactionAsync();

        try
        {
            var ids = request.FinancialInvoices.Select(x => x.InvoiceId).ToList();

            var invs = await _RepositoryInvoice
                .GetListByFilterAsync(e => ids.Contains(e.Id), "");

            if (invs == null)
                invs = new List<Invoice>();

            foreach (var inv in invs)
            {
                var amount = request.FinancialInvoices
                    .Where(e => e.InvoiceId == inv.Id)
                    .Sum(e => e.Amount);

                inv.Credit = (inv.Net - amount) - inv.Paid;
                inv.Paid = (inv.Net - inv.Credit);

                await _RepositoryInvoice.UpdateAsync(inv);
            }

            var result = await base.Handle(request, cancellationToken);

            await _UnitOfWork.CommitAsync();

            return result;
        }
        catch (Exception)
        {
            await _UnitOfWork.RollbackAsync();
            throw;
        }
    }

}