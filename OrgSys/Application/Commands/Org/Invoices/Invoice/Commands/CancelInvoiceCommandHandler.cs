using Application.Abstraction.Command;
using Application.Common.Commands;
using Application.Interfaces.CQRS;
using AutoMapper;
using Domain.Abstraction;
using Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Application.Commands.Org.Invoices.Invoice.Commands
{
    public record CancelInvoiceCommand(long Id) : ICommand, IUpdateCommand<Result>;

    public class CancelInvoiceCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Invoice> _Repository, IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateInvoiceCommand, Entity.Model.Invoice>(_UnitOfWork, _Repository, mapper, _provider)
    {

        public override async Task<Result> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var invoice = await _Repository.GetByFilterAsync(x => x.Id == request.Id, await CreateInclude());
                if (invoice is null)
                    return new Result(HttpStatusCode.NotFound, new List<Error> { new Error("Invoice not found") });
                invoice.Status = Utility.Status.Cancel;
                if (await _UnitOfWork.SaveChangeAsync() > 0)
                {
                    return new Result(HttpStatusCode.OK, null);
                }

                return new Result(HttpStatusCode.InternalServerError, new List<Error> { new Error("Error") });
            }
            catch (Exception ex)
            {
                return new Result(
                  HttpStatusCode.InternalServerError,
                  new List<Error> { new Error(ex.Message) });
            }
        }       
    }
}