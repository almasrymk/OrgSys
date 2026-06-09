using Application.Abstraction.Command;
using Application.Common.Commands;
using Application.Interfaces.CQRS;
using AutoMapper;
using Domain.Abstraction;
using Domain.Shared;
using Entity.Model;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;


namespace Application.Commands.Org.Financials.Financial.Commands
{
    public record RedoFinancialCommand(long Id) : ICommand, IUpdateCommand<Result>;

    public class RedoFinancialCommandHandler(IUnitOfWork _UnitOfWork,
        IRepository<Entity.Model.Financial> _Repository, 
        IRepository<Entity.Model.Invoice> _RepositoryInvoice, 
        IMapper mapper, IServiceProvider _provider
        ) 
        : UpdateCommandHandler<RedoFinancialCommand, Entity.Model.Financial>(_UnitOfWork, _Repository, mapper, _provider)
    {
        public override async Task<Result> Handle(RedoFinancialCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var finanicial = await _Repository
                    .GetByFilterAsync(e => e.Id == request.Id, "FinancialInvoices") ?? new();

                foreach (var item in finanicial.FinancialInvoices)
                {
                    var invoice = await _RepositoryInvoice
                        .GetByFilterAsync(e => e.Id == item.InvoiceId, "");

                    if (invoice == null)
                        continue;

                    invoice.Credit -= item.Amount;
                    invoice.Paid += item.Amount;

                    item.Status = Utility.Status.All;

                    await _RepositoryInvoice.UpdateAsync(invoice);
                }

                finanicial.Status = Utility.Status.All;

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
