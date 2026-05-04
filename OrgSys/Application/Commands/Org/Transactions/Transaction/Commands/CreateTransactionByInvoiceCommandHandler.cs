namespace Application.Commands.Org.Invoices.Invoice.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.Model;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class CreateTransactionByInvoiceCommand : Entity.ModelView.InvoiceModelView, ICommand, ICreateCommand<Result>;

    public sealed class CreateTransactionByInvoiceCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Transaction> _Repository, IMapper mapper) : CreateCommandHandler<CreateTransactionByInvoiceCommand, Entity.Model.Transaction>(_UnitOfWork, _Repository, mapper)
    {
        public override async Task<Result> Handle(CreateTransactionByInvoiceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var ob = mapper.Map<Transaction>(request);
                await FixData(ob, request);
                var res = await _Repository.CreateAsync(ob);
                if (_UnitOfWork.SaveChangeAsync().Result > 0)
                {
                    return new Result(HttpStatusCode.OK, null);
                }

                return new Result(
                    HttpStatusCode.InternalServerError,
                    new List<Error> { new Error("Error") });
            }
            catch (Exception ex)
            {
                return new Result(
                    HttpStatusCode.InternalServerError,
                    new List<Error> { new Error(ex.Message) });
            }
        }

        private async Task<Transaction> FixData(Transaction transaction, CreateTransactionByInvoiceCommand request)
        {
            transaction.TypeId = request.TypeId == 2 || request.TypeId == 3 ? 1 : 2;
            var transactionOld = await _Repository.GetByFilterAsync(e => e.Id == request.TransactionId, "");
            if (transactionOld == null || transactionOld.Id == 0)
            {
                transaction.Id = 0;
                transaction.CodeNumber = await _Repository.GetMaxByFilterAsync(e => e.TypeId == (request.TypeId == 2 || request.TypeId == 3 ? 1 : 2), e => e.CodeNumber);
                transaction.Code = "" + transaction.CodeNumber;
                transaction.ParentId = transaction.ParentId;
            }
            else
            {
                transaction.Id = transactionOld.Id;
                transaction.CodeNumber = transactionOld.CodeNumber;
                transaction.Code = "" + transactionOld.Code;
            }

            transaction.Stock = null;
            transaction.Dealer = null;
            foreach (var item in transaction.TransactionProducts)
            {
                item.Id = 0;
                item.Product = null;
                item.Unit = null;
            }
            return transaction;
        }
    }
}