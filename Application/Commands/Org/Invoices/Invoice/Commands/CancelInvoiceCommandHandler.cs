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
    public record CancelInvoiceCommand(long Id) : ICommand , ICancelCommand;

    public class CancelInvoiceCommandHandler(IUnitOfWork unitOfWork,IRepository<Entity.Model.Invoice> repository , IMapper mapper, IServiceProvider provider) : UpdateCommandHandler<CancelInvoiceCommand, Entity.Model.Invoice>(unitOfWork, repository, mapper, provider)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IRepository<Entity.Model.Invoice> _repository = repository;

        public override async Task<Result> Handle(CancelInvoiceCommand request, CancellationToken ct)
        {

            try
            {
                var invoice = await _repository.GetByFilterAsync(x => x.Id == request.Id, await CreateInclude());
                if (invoice is null)
                    return new Result(HttpStatusCode.NotFound, new List<Error> { new Error("Invoice not found") });
                invoice.Status = Utility.Status.Cancel;
                if (await _unitOfWork.SaveChangeAsync() > 0)
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
