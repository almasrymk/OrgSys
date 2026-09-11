namespace Inventory.Application.Transactions.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using global::Application.Commands.Org.Financials.Integration.JournalTransaction;

    public record CreateTransactionByInvoiceCommand(long Id, bool RespectAutoCreatePreference = false)
    : ICommand, ICreateCommand<Result>;

    public sealed class CreateTransactionByInvoiceCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Transaction> _Repository,
        IRepository<Invoice> _InvoiceRepository, IRepository<Preference> preferenceRepository, IMapper mapper,
        IServiceProvider provider) : CreateCommandHandler<CreateTransactionByInvoiceCommand, Transaction>(_UnitOfWork, _Repository, mapper)
    {
        public override async Task<Result> Handle(CreateTransactionByInvoiceCommand request, CancellationToken cancellationToken)
        {
            await _UnitOfWork.BeginTransactionAsync();
            try
            {
                var invoice = await _InvoiceRepository.GetByFilterAsync(e => e.Id == request.Id,"InvoiceProducts");

                if (invoice == null)
                {
                    await _UnitOfWork.RollbackAsync();
                    return new Result(HttpStatusCode.NotFound,new List<Error>{new Error("Invoice not found")});
                }

                if (request.RespectAutoCreatePreference && invoice.TransactionId is not > 0)
                {
                    var autoCreateTransaction = await preferenceRepository.GetByFilterAsync(
                        e => e.Reference == "Invoice"
                            && e.TypeId == invoice.TypeId
                            && e.Key == "AutoCreateTransaction",
                        "");

                    if (autoCreateTransaction?.Value != "1")
                    {
                        await _UnitOfWork.RollbackAsync();
                        return new Result(HttpStatusCode.OK, null);
                    }
                }
                
                Transaction transaction;
                var expectedTransactionTypeId = invoice.TypeId == 2 || invoice.TypeId == 3 ? 1L : 2L;
                var transactionTypeChanged = false;

                if (invoice.TransactionId > 0)
                {
                    transaction = (await _Repository.GetByFilterAsync(e => e.Id == invoice.TransactionId,"TransactionProducts"))!;

                    if (transaction == null)
                    {
                        await _UnitOfWork.RollbackAsync();
                        return new Result(HttpStatusCode.NotFound,new List<Error>{new Error("Transaction not found")});
                    }

                    var originalTransactionTypeId = transaction.TypeId;
                    transaction.TransactionProducts?.Clear();

                    mapper.Map(invoice, transaction);
                    transaction.TypeId = expectedTransactionTypeId;
                    transactionTypeChanged = originalTransactionTypeId != expectedTransactionTypeId;

                }
                else
                {
                    transaction = mapper.Map<Transaction>(invoice);

                    transaction.TypeId = expectedTransactionTypeId;

                    transaction.CodeNumber = await _Repository.AnyAsync(e =>
                            e.TypeId == transaction.TypeId
                            || (transaction.TypeId == 1 && e.TypeId == 5)
                            || (transaction.TypeId == 2 && e.TypeId == 6))
                        ? await _Repository.GetMaxByFilterAsync(
                            e => e.TypeId == transaction.TypeId
                                || (transaction.TypeId == 1 && e.TypeId == 5)
                                || (transaction.TypeId == 2 && e.TypeId == 6),
                            e => e.CodeNumber) + 1
                        : 1;

                    transaction.Code = transaction.CodeNumber.ToString();
                }


                if (invoice.TransactionId == null || invoice.TransactionId == 0)
                    await _Repository.CreateAsync(transaction);
                

                if (await _UnitOfWork.SaveChangeAsync() > 0)
                {
                    invoice.TransactionId = transaction.Id;

                    if (transactionTypeChanged)
                        await new TransactionJournalIntegration(provider).DeleteByTransactionIdAsync(transaction.Id);
                    await new TransactionJournalIntegration(provider).SyncAsync(transaction, invoice.TypeId);
                    await _Repository.UpdateAsync(transaction);

                    await _UnitOfWork.SaveChangeAsync(cancellationToken);
                    await _UnitOfWork.CommitAsync();

                    return new Result(HttpStatusCode.OK, null);
                }

                await _UnitOfWork.RollbackAsync();
                return new Result(HttpStatusCode.InternalServerError,new List<Error>{new Error("Error while saving")});
            }
            catch (Exception ex)
            {
                await _UnitOfWork.RollbackAsync();
                return new Result(HttpStatusCode.InternalServerError,new List<Error>{new Error(ex.Message)});
            }
        }
    }
}
