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


namespace Application.Commands.Org.Invoices.Invoice.Commands
{
    public record RedoInvoiceCommand(long Id) : ICommand, IUpdateCommand<Result>;

    public class RedoInvoiceCommandHandler(IUnitOfWork _UnitOfWork,
        IRepository<Entity.Model.Invoice> _Repository, 
        IMapper mapper, IServiceProvider _provider
        ) 
        : UpdateCommandHandler<RedoInvoiceCommand, Entity.Model.Invoice>(_UnitOfWork, _Repository, mapper, _provider)
    {

        public override async Task<Result> Handle(RedoInvoiceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var invoice = await _Repository.GetByFilterAsync(x => x.Id == request.Id, await CreateInclude());


                if (invoice is null)
                    return new Result(HttpStatusCode.NotFound, new List<Error> { new Error("Invoice not found") });

                invoice.Status = Utility.Status.All;

                if (invoice.TransactionId > 0)
                {
                    var transactionRepo = _provider.GetRequiredService<IRepository<Entity.Model.Transaction>>();

                    var transaction = await transactionRepo.GetByFilterAsync(x => x.Id == invoice.TransactionId, await CreateInclude());

                    if (transaction == null)
                        return new Result(HttpStatusCode.InternalServerError, new List<Error> { new Error("Transaction not found") });

                    transaction.Status = Utility.Status.All;
                }

                var saved = await _UnitOfWork.SaveChangeAsync();

                return saved > 0 ? new Result(HttpStatusCode.OK, null) : new Result(HttpStatusCode.InternalServerError, new List<Error> { new Error("Error saving changes") });
            }
            catch (Exception ex)
            {

                return new Result(HttpStatusCode.InternalServerError, new List<Error> { new Error("Error") });

            }
        }
    }
}
