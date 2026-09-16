namespace Treasury.Application.Financials.Commands
{
    using Administration.Contracts.Preferences;
    using AutoMapper;
    using CommercialDocuments.Contracts.Invoices;
    using System.Net;
    using MediatR;

    public sealed class UpdateFinancialCommand : Treasury.Application.FinancialDto, ICommand, IUpdateCommand<Result>;
    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork,
        IRepository<Treasury.Domain.Financial> _Repository ,
        IRepository<Treasury.Domain.FinancialInvoice> _RepositoryFinancialInvoice,
        IMapper mapper, IServiceProvider _provider, ISender sender) : UpdateCommandHandler<UpdateFinancialCommand, Treasury.Domain.Financial>(_UnitOfWork, _Repository , mapper , _provider)
    {
        public override async Task<Result> Handle(UpdateFinancialCommand request, CancellationToken cancellationToken)
        {
            var finanicial = await _Repository.GetByFilterAsync(e => e.Id == request.Id, "FinancialInvoices") ?? new Financial();

            if (finanicial.Posted)
                return new Result(HttpStatusCode.Forbidden, [new Error("A Posted financial transaction cannot be edited. Use Reverse instead.")]);

            foreach (var item in finanicial.FinancialInvoices ?? [])
                await sender.Send(new AdjustInvoiceSettlementCommand(item.InvoiceId ?? 0, -item.Amount), cancellationToken);

            foreach (var item in request.FinancialInvoices ?? [])
                await sender.Send(new AdjustInvoiceSettlementCommand(item.InvoiceId ?? 0, item.Amount), cancellationToken);

            finanicial.FinancialInvoices ??= new List<Treasury.Domain.FinancialInvoice>();
            finanicial.FinancialInvoices.Clear();
            await _RepositoryFinancialInvoice.CreateAsync((request.FinancialInvoices ?? []).ToList());
            var result = await base.Handle(request, cancellationToken);

            if (result.StatusCode == HttpStatusCode.OK
                && request.FinancialTypeId == (long)Treasury.Domain.FinancialTransactionType.OpeningBalance)
            {
                var autoPost = (await sender.Send(
                    new GetPreferenceValueQuery("Financial", request.TypeId, "AutoCreateJournalEntry"), cancellationToken)).Response;
                if (autoPost == "1")
                    return await sender.Send(new PostFinancialOpeningBalanceCommand(request.Id, request.ModifyUserId ?? request.CreateUserId), cancellationToken);
            }

            return result;
        }
    }
}
