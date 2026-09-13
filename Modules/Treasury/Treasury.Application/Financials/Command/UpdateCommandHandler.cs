namespace Treasury.Application.Financials.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Net;
    using MediatR;

    public sealed class UpdateFinancialCommand : Treasury.Application.FinancialDto, ICommand, IUpdateCommand<Result>;
    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork,
        IRepository<Treasury.Domain.Financial> _Repository ,
        IRepository<Treasury.Domain.FinancialInvoice> _RepositoryFinancialInvoice,
        IRepository<CommercialDocuments.Domain.Invoice> _RepositoryInvoice,
        IRepository<Preference> _PreferenceRepository,
        IMapper mapper, IServiceProvider _provider, ISender sender) : UpdateCommandHandler<UpdateFinancialCommand, Treasury.Domain.Financial>(_UnitOfWork, _Repository , mapper , _provider)
    {

        public override async Task<Result> Handle(UpdateFinancialCommand request, CancellationToken cancellationToken)
        {
            //_RepositoryFinancialInvoice.GetListByFilterAsync()
            //await _UnitOfWork.BeginTransactionAsync();

            var finanicial = await _Repository.GetByFilterAsync(e => e.Id == request.Id, "FinancialInvoices") ?? new Financial();

            // A Posted financial transaction (e.g. a Customer Receipt) is immutable — correct it via
            // Reverse, never by editing the row a Posted Journal Entry was already generated from.
            if (finanicial.Posted)
                return new Result(HttpStatusCode.Forbidden, [new Error("A Posted financial transaction cannot be edited. Use Reverse instead.")]);



            foreach (var item in finanicial.FinancialInvoices ?? [])
            {
                var invoice = await _RepositoryInvoice.GetByFilterAsync(e => e.Id == item.InvoiceId,"");
                invoice!.Credit += item.Amount;
                invoice.Paid -= item.Amount;
               await _RepositoryInvoice.UpdateAsync(invoice);
            }

            foreach (var item in request.FinancialInvoices ?? [])
            {
                var invoice = await _RepositoryInvoice.GetByFilterAsync(e => e.Id == item.InvoiceId, "");
                invoice!.Credit -= item.Amount;
                invoice.Paid += item.Amount;
                await _RepositoryInvoice.UpdateAsync(invoice);
            }

            finanicial.FinancialInvoices ??= new List<Treasury.Domain.FinancialInvoice>();
            finanicial.FinancialInvoices.Clear();
            await _RepositoryFinancialInvoice.CreateAsync((request.FinancialInvoices ?? []).ToList());
            var result = await base.Handle(request, cancellationToken);

            // Opening Balance only: same Auto-Create Journal Entry preference the Create side honors —
            // re-saving an untouched/corrected Draft posts it immediately instead of requiring a
            // separate manual Post click.
            if (result.StatusCode == HttpStatusCode.OK
                && request.FinancialTypeId == (long)Treasury.Domain.FinancialTransactionType.OpeningBalance)
            {
                var autoPost = await _PreferenceRepository.GetByFilterAsync(
                    e => e.Reference == "Financial" && e.TypeId == request.TypeId && e.Key == "AutoCreateJournalEntry", "");
                if (autoPost?.Value == "1")
                    return await sender.Send(new PostFinancialOpeningBalanceCommand(request.Id, request.ModifyUserId ?? request.CreateUserId), cancellationToken);
            }

            return result;
        }

    }
}