namespace Accounting.Application.FiscalYears.Commands
{
    using Accounting.Domain.Repositories;
    using OrgSys.SharedKernel;
    using System.Net;

    public sealed record CloseFiscalPeriodCommand(long Id) : ICommand, IUpdateCommand<Result>;

    public sealed class CloseFiscalPeriodCommandHandler(
        IUnitOfWork unitOfWork,
        IFiscalPeriodRepository repository) : ICommandHandler<CloseFiscalPeriodCommand>
    {
        public async Task<Result> Handle(CloseFiscalPeriodCommand request, CancellationToken cancellationToken)
        {
            var period = await repository.GetByIdAsync(request.Id, cancellationToken);
            if (period is null)
                return new Result(HttpStatusCode.NotFound, [new Error("Fiscal period not found")]);

            period.Close();

            return await unitOfWork.SaveChangeAsync(cancellationToken) >= 0
                ? new Result(HttpStatusCode.OK, null)
                : new Result(HttpStatusCode.InternalServerError, [new Error("Error saving changes")]);
        }
    }

    public sealed record ReopenFiscalPeriodCommand(long Id) : ICommand, IUpdateCommand<Result>;

    public sealed class ReopenFiscalPeriodCommandHandler(
        IUnitOfWork unitOfWork,
        IFiscalPeriodRepository repository) : ICommandHandler<ReopenFiscalPeriodCommand>
    {
        public async Task<Result> Handle(ReopenFiscalPeriodCommand request, CancellationToken cancellationToken)
        {
            var period = await repository.GetByIdAsync(request.Id, cancellationToken);
            if (period is null)
                return new Result(HttpStatusCode.NotFound, [new Error("Fiscal period not found")]);

            try
            {
                period.Reopen();
            }
            catch (Accounting.Domain.Exceptions.AccountingPeriodClosedException ex)
            {
                return new Result(HttpStatusCode.BadRequest, [new Error(ex.Message)]);
            }

            return await unitOfWork.SaveChangeAsync(cancellationToken) >= 0
                ? new Result(HttpStatusCode.OK, null)
                : new Result(HttpStatusCode.InternalServerError, [new Error("Error saving changes")]);
        }
    }
}
