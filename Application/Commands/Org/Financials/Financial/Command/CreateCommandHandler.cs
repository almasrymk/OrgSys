namespace Application.Commands.Org.Financials.Financial.Commands;

using Application.Abstraction.Command;
using Application.Common.Commands;
using Application.Interfaces.CQRS;
using AutoMapper;
using Domain.Abstraction;
using Domain.Shared;
using Domain.Entities;
using Application.DTOs;
using System.Net;
using MediatR;

public sealed class CreateFinancialCommand : Application.DTOs.FinancialDto, ICommand , ICreateCommand<Result>;

public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork,
    IRepository<Domain.Entities.Financial> _Repository,
    IRepository<Invoice> _RepositoryInvoice,
    IRepository<FinancialInvoice> _RepositoryFinancialInvoice,
    IRepository<Preference> _PreferenceRepository,
    IMapper mapper,
    ISender sender) : CreateCommandHandler<CreateFinancialCommand, Domain.Entities.Financial>(_UnitOfWork, _Repository , mapper)
{

    public override async Task<Result> Handle(CreateFinancialCommand request, CancellationToken cancellationToken)
    {
        await _UnitOfWork.BeginTransactionAsync();

        try
        {
            var ids = (request.FinancialInvoices ?? []).Select(x => x.InvoiceId).ToList();

            var invs = await _RepositoryInvoice
                .GetListByFilterAsync(e => ids.Contains(e.Id), "");

            if (invs == null)
                invs = new List<Invoice>();

            foreach (var inv in invs)
            {
                var amount = request.FinancialInvoices
                    .Where(e => e.InvoiceId == inv.Id)
                    .Sum(e => e.Amount);

                inv.Credit = (inv.Net - amount) - inv.Paid;
                inv.Paid = (inv.Net - inv.Credit);

                await _RepositoryInvoice.UpdateAsync(inv);
            }

            var result = await base.Handle(request, cancellationToken);

            await _UnitOfWork.CommitAsync();

            // Opening Balance only: when Preferences > Financial > Opening Balance has "Auto-Create
            // Journal Entry" on, post immediately instead of leaving the Draft for a separate manual
            // Post click — same idea as Invoice's own AutoCreateJournalEntry preference. Runs after the
            // Draft's own transaction commits, in a new transaction of its own (PostFinancialOpeningBalanceCommandHandler
            // manages that itself), so a failed auto-post never rolls back the successfully-saved Draft.
            if (result.StatusCode == HttpStatusCode.OK
                && CreatedEntity is not null
                && request.FinancialTypeId == (long)Domain.Enums.FinancialTransactionType.OpeningBalance)
            {
                var autoPost = await _PreferenceRepository.GetByFilterAsync(
                    e => e.Reference == "Financial" && e.TypeId == request.TypeId && e.Key == "AutoCreateJournalEntry", "");
                if (autoPost?.Value == "1")
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