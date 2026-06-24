namespace Application.Commands.Org.Invoices.Invoice.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Domain.Entities;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    public record CreateTransactionByInvoiceCommand(long Id)
    : ICommand, ICreateCommand<Result>;

    public sealed class CreateTransactionByInvoiceCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Transaction> _Repository, IRepository<Invoice> _InvoiceRepository, IMapper mapper) : CreateCommandHandler<CreateTransactionByInvoiceCommand, Transaction>(_UnitOfWork, _Repository, mapper)
    {
        public override async Task<Result> Handle(CreateTransactionByInvoiceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var invoice = await _InvoiceRepository.GetByFilterAsync(e => e.Id == request.Id,"InvoiceProducts");

                if (invoice == null)
                    return new Result(HttpStatusCode.NotFound,new List<Error>{new Error("Invoice not found")});
                
                Transaction transaction;

                if (invoice.TransactionId > 0)
                {
                    transaction = (await _Repository.GetByFilterAsync(e => e.Id == invoice.TransactionId,"TransactionProducts"))!;

                    if (transaction == null)
                        return new Result(HttpStatusCode.NotFound,new List<Error>{new Error("Transaction not found")});

                    transaction.TransactionProducts?.Clear();

                    mapper.Map(invoice, transaction);

                }
                else
                {
                    transaction = mapper.Map<Transaction>(invoice);

                    transaction.TypeId = invoice.TypeId == 2 || invoice.TypeId == 3 ? 1 : 2;

                    transaction.CodeNumber =await _Repository.GetMaxByFilterAsync(e => e.TypeId == transaction.TypeId,e => e.CodeNumber) + 1;

                    transaction.Code = transaction.CodeNumber.ToString();
                }


                if (invoice.TransactionId == null || invoice.TransactionId == 0)
                    await _Repository.CreateAsync(transaction);
                

                if (await _UnitOfWork.SaveChangeAsync() > 0)
                {
                    invoice.TransactionId = transaction.Id;

                    await _UnitOfWork.SaveChangeAsync();

                    return new Result(HttpStatusCode.OK, null);
                }

                return new Result(HttpStatusCode.InternalServerError,new List<Error>{new Error("Error while saving")});
            }
            catch (Exception ex)
            {
                return new Result(HttpStatusCode.InternalServerError,new List<Error>{new Error(ex.Message)});
            }
        }
    }
}