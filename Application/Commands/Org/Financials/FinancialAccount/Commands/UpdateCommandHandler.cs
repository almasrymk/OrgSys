namespace Application.Commands.Org.Financials.FinancialAccount.Commands
{
    using Application.Abstraction.Command;
    using Application.Commands.Org.Financials.Unified;
    using Application.Interfaces.CQRS;
    using Domain.Shared;
    using Application.DTOs;
    using MediatR;

    public sealed class UpdateFinancialAccountCommand : FinancialAccountDto, ICommand, IUpdateCommand<Result>;

    // Same reasoning as CreateCommandHandler: delegate to the existing unified Save handler rather than
    // re-deriving the CashBox/BankAccount dual-write through the generic UpdateCommandHandler<> base.
    public sealed class UpdateCommandHandler(ISender sender) : ICommandHandler<UpdateFinancialAccountCommand>
    {
        public Task<Result> Handle(UpdateFinancialAccountCommand request, CancellationToken cancellationToken) =>
            sender.Send(new SaveFinancialAccountCommand(request), cancellationToken);
    }
}
