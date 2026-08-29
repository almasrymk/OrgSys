using Application.Abstraction.Command;
using Application.Common.Commands;
using Application.Interfaces.CQRS;
using AutoMapper;
using Domain.Abstraction;
using Domain.Shared;
using Domain.Entities;
using Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Net;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Application.Commands.Org.Financials.Financial.Command
{
    public sealed record CreateFinancialPaidInvoiceCommand(long InvoiceId) : ICommand, ICreateCommand<Result>;


    public sealed class CreateFinancialPaidInvoiceCommandHandler(IUnitOfWork _UnitOfWork, 
        IRepository<Domain.Entities.Financial> _Repository, IServiceProvider _Provider, IMapper mapper) :
        CreateCommandHandler<CreateFinancialPaidInvoiceCommand, Domain.Entities.Financial>(_UnitOfWork, _Repository, mapper)
    {


        public override async Task<Result> Handle(CreateFinancialPaidInvoiceCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var invoiceRepo = _Provider.GetRequiredService<IRepository<Domain.Entities.Invoice>>();

                var invoice = await invoiceRepo.GetByFilterAsync(x => x.Id == request.InvoiceId, "");

                if (invoice is null)
                    return new Result(HttpStatusCode.InternalServerError, new List<Error> { new Error("Invoice not found") });

                if (invoice.Credit > 0)
                {
                    var financial = mapper.Map<Domain.Entities.Financial>(invoice);
                    financial.Id = 0;
                    financial.Dealer = null;
                    financial.Amount = invoice.Credit;
                    financial.Rate = invoice.Rate > 0 ? invoice.Rate : (invoice.Currency?.Rate ?? 0);
                    financial.AmountByDefaultCurrency = invoice.Credit * financial.Rate;
                    financial.CreateDate = DateTime.Now;
                    financial.TypeId = invoice.TypeId == 1 || invoice.TypeId == 4 ? 1 : 2;

                    var prefRepo = _Provider.GetRequiredService<IRepository<Domain.Entities.Preference>>();

                    var cashBoxPref = await prefRepo.GetByFilterAsync(
                        e => e.Key == "DefaultCashBox"
                        && (e.TypeId == (invoice.TypeId == 1 || invoice.TypeId == 4 ? 1 : 2)
                            || (invoice.TypeId == 1 || invoice.TypeId == 4 ? 1 : 2) == 0)
                        && (e.Reference == "Financial" || "Financial" == ""),
                        ""
                    );

                    financial.FinancialAccountId = int.Parse(cashBoxPref?.Value ?? "0");

                    //invoice.CodeNumber = await _Repository.GetMaxByFilterAsync(e => e.TypeId == invoice.TypeId, e => e.CodeNumber) + 1;

                    //invoice.Code = invoice.CodeNumber.ToString();

                    //FinancialInvoice financialInvoice = mapper.Map<Domain.Entities.FinancialInvoice>(invoice);
                    FinancialInvoice financialInvoice = new FinancialInvoice();
                    financialInvoice.Id = 0;
                    financialInvoice.TypeId = invoice.TypeId == 1 || invoice.TypeId == 4 ? 1 : 2;
                    financialInvoice.Amount = invoice.Credit;
                    financialInvoice.InvoiceId = invoice.Id;
                    financial.FinancialInvoices = new List<FinancialInvoice>();
                    financial.FinancialInvoices.Add(financialInvoice);
                    invoice.Credit = invoice.Net - invoice.Paid - financial.Amount;
                    invoice.Paid = invoice.Net - invoice.Credit;

                    await _Repository.CreateAsync(financial);
                    await _UnitOfWork.SaveChangeAsync();

                }
            return new Result(HttpStatusCode.OK, new List<Error>());
            }
            catch (Exception ex)
            {
                return new Result(HttpStatusCode.InternalServerError,new List<Error>{new Error(ex.Message)});
            }
           

        }
    }
}
