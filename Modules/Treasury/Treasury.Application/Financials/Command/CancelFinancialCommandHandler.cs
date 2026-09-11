using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Transactions;

namespace Treasury.Application.Financials.Commands
{
    public record CancelFinancialCommand(long Id) : ICommand, IUpdateCommand<Result>;

    public class CancelFinancialCommandHandler(IUnitOfWork _UnitOfWork, 
        IRepository<Treasury.Domain.Financial> _Repository,
        IRepository<Sales.Domain.Invoice> _RepositoryInvoice,
        IMapper mapper, IServiceProvider _provider) :
        UpdateCommandHandler<CancelFinancialCommand, Treasury.Domain.Financial>(_UnitOfWork, _Repository, mapper, _provider)
    {

        public override async Task<Result> Handle(CancelFinancialCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var finanicial = await _Repository.GetByFilterAsync(e => e.Id == request.Id, "FinancialInvoices") ?? new();

                if (finanicial.Posted)
                    return new Result(HttpStatusCode.Forbidden, [new Error("A Posted financial transaction cannot be cancelled. Use Reverse instead.")]);

                foreach (var item in finanicial.FinancialInvoices ?? [])
                {
                    var invoice = await _RepositoryInvoice.GetByFilterAsync(e => e.Id == item.InvoiceId, "");
                    invoice!.Credit += item.Amount;
                    invoice.Paid -= item.Amount;
                    item.Status = OrgSys.SharedKernel.Status.Cancel;

                    await _RepositoryInvoice.UpdateAsync(invoice);
                }
                finanicial.Status = OrgSys.SharedKernel.Status.Cancel;

                await _UnitOfWork.SaveChangeAsync();

                return new Result(HttpStatusCode.OK, null);

            }
            catch (Exception ex)
            {
                return new Result(
                HttpStatusCode.InternalServerError,
                new List<Error> { new Error("Error") });
            }

        }
    }
}