namespace Treasury.Application.Financials.Commands;

using Administration.Contracts.Preferences;
using AutoMapper;
using CommercialDocuments.Contracts.Invoices;
using System.Net;
using MediatR;

public sealed class CreateFinancialCommand : Treasury.Application.FinancialDto, ICommand , ICreateCommand<Result>;

public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork,
    IRepository<Treasury.Domain.Financial> _Repository,
    IRepository<FinancialInvoice> _RepositoryFinancialInvoice,
    IMapper mapper,
    ISender sender) : CreateCommandHandler<CreateFinancialCommand, Treasury.Domain.Financial>(_UnitOfWork, _Repository , mapper)
{

    public override async Task<Result> Handle(CreateFinancialCommand request, CancellationToken cancellationToken)
    {
        await _UnitOfWork.BeginTransactionAsync();

        try
        {
            foreach (var item in request.FinancialInvoices ?? [])
            {
                await sender.Send(new SetInvoiceSettlementFromAllocationCommand(item.InvoiceId ?? 0, item.Amount), cancellationToken);
            }

            var result = await base.Handle(request, cancellationToken);

            await _UnitOfWork.CommitAsync();

            if (result.StatusCode == HttpStatusCode.OK
                && CreatedEntity is not null
                && request.FinancialTypeId == (long)Treasury.Domain.FinancialTransactionType.OpeningBalance)
            {
                var autoPost = (await sender.Send(
                    new GetPreferenceValueQuery("Financial", request.TypeId, "AutoCreateJournalEntry"), cancellationToken)).Response;
                if (autoPost == "1")
                    return await sender.Send(new PostFinancialOpeningBalanceCommand(CreatedEntity.Id, request.CreateUserId), cancellationToken);
            }

            return result;
        }
        catch (Exception)
        {
            await _UnitOfWork.RollbackAsync();
            throw;
        }
    }

}
