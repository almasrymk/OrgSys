namespace Accounting.Application.Accounts.Commands
{
    using OrgSys.SharedKernel;
    using System.Net;

    public sealed record DeactivateAccountCommand(long Id) : ICommand, IUpdateCommand<Result>;

    public sealed class DeactivateCommandHandler(
        IUnitOfWork unitOfWork,
        IRepository<Accounting.Domain.Account> repository) : ICommandHandler<DeactivateAccountCommand>
    {
        public async Task<Result> Handle(DeactivateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await repository.GetByFilterAsync(a => a.Id == request.Id, string.Empty);
            if (account is null)
                return new Result(HttpStatusCode.NotFound, [new Error("Account not found")]);

            account.Deactivate();

            return await unitOfWork.SaveChangeAsync(cancellationToken) >= 0
                ? new Result(HttpStatusCode.OK, null)
                : new Result(HttpStatusCode.InternalServerError, [new Error("Error saving changes")]);
        }
    }

    public sealed record ActivateAccountCommand(long Id) : ICommand, IUpdateCommand<Result>;

    public sealed class ActivateCommandHandler(
        IUnitOfWork unitOfWork,
        IRepository<Accounting.Domain.Account> repository) : ICommandHandler<ActivateAccountCommand>
    {
        public async Task<Result> Handle(ActivateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await repository.GetByFilterAsync(a => a.Id == request.Id, string.Empty);
            if (account is null)
                return new Result(HttpStatusCode.NotFound, [new Error("Account not found")]);

            account.Activate();

            return await unitOfWork.SaveChangeAsync(cancellationToken) >= 0
                ? new Result(HttpStatusCode.OK, null)
                : new Result(HttpStatusCode.InternalServerError, [new Error("Error saving changes")]);
        }
    }
}
