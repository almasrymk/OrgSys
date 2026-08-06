namespace Application.Commands.Org.Transactions.Transaction.Commands
{
    using Application.Abstraction.Command;
    using Application.Commands.Org.Invoices.Invoice.Commands;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Domain.Entities;
    using Application.DTOs;
    using Application.Commands.Org.Financials.Integration.JournalTransaction;

    public sealed class UpdateTransactionCommand : Application.DTOs.TransactionDto , ICommand, IUpdateCommand<Result>;
    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork,
        IRepository<Domain.Entities.Transaction> _Repository , 
        IRepository<TransactionProduct> _TransactionRepository,
        IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateTransactionCommand, Domain.Entities.Transaction>(_UnitOfWork, _Repository , mapper , _provider)
    {
        override public async Task<bool> SaveDetials(UpdateTransactionCommand request)
        {
            #region UpdateProduct
            var ids = request.TransactionProductList.Select(e => e.Id);
            var removeList = await _TransactionRepository.GetListByFilterAsync(e => e.TransactionId == request.Id && !ids.Contains(e.Id));

            var res = await RemoveDetails<TransactionProduct>(removeList!);
            if (!res) return false;
            var ob = mapper.Map<List<TransactionProduct>>(request.TransactionProductList);
            res = await UpdateDetails<TransactionProduct>(ob);
            #endregion 

            if (res)
            {
                await new TransactionJournalIntegration(_provider).SyncAsync(request);
                res = await _Repository.UpdateAsync(request);
            }

            return res;
        }
    }
}
