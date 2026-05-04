using Application.Abstraction.Command;
using Application.Common.Commands;
using Application.Interfaces.CQRS;
using AutoMapper;
using Domain.Abstraction;
using Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Transactions;

namespace Application.Commands.Org.Invoices.Invoice.Commands
{
    public record CancelInvoiceCommand(long Id) : ICommand, IUpdateCommand<Result>;

    public class CancelInvoiceCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Invoice> _Repository, IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<CancelInvoiceCommand, Entity.Model.Invoice>(_UnitOfWork, _Repository, mapper, _provider)
    {

        public override async Task<Result> Handle(CancelInvoiceCommand request, CancellationToken cancellationToken)
        {
            return await UpdateInvoiceWithTransaction(request.Id, Utility.Status.Cancel);
        }
    }
}