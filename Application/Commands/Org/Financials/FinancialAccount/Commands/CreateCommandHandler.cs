namespace Application.Commands.Org.Financials.FinancialAccount.Commands
{
    using Application.Abstraction.Command;
    using Application.Commands.Org.Financials.Unified;
    using Application.Interfaces.CQRS;
    using Domain.Shared;
    using Application.DTOs;
    using MediatR;

    public sealed class CreateFinancialAccountCommand : FinancialAccountDto, ICommand, ICreateCommand<Result>;

    // Delegates to SaveFinancialAccountCommandHandler (Application/Commands/Org/Financials/UnifiedFinancialCommands.cs),
    // which already knows how to write the CashBox/BankAccount detail row alongside the FinancialAccount header —
    // re-implementing that dual-entity write here via the generic CreateCommandHandler<> base would either drop the
    // detail row or duplicate the exact same logic.
    public sealed class CreateCommandHandler(ISender sender) : ICommandHandler<CreateFinancialAccountCommand>
    {
        public Task<Result> Handle(CreateFinancialAccountCommand request, CancellationToken cancellationToken) =>
            sender.Send(new SaveFinancialAccountCommand(request), cancellationToken);
    }
}
