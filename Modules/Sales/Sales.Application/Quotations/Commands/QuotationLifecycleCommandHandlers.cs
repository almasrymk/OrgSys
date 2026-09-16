namespace Sales.Application.Quotations.Commands;

using System.Net;

public sealed record SendQuotationCommand(long Id) : ICommand;
public sealed record AcceptQuotationCommand(long Id) : ICommand;
public sealed record RejectQuotationCommand(long Id) : ICommand;
public sealed record CancelQuotationCommand(long Id) : ICommand;

public sealed class SendQuotationCommandHandler(IRepository<Quotation> repository, IUnitOfWork unitOfWork)
    : ICommandHandler<SendQuotationCommand>
{
    public Task<Result> Handle(SendQuotationCommand request, CancellationToken cancellationToken) =>
        QuotationLifecycle.Apply(repository, unitOfWork, request.Id, q => q.Send(), cancellationToken);
}

public sealed class AcceptQuotationCommandHandler(IRepository<Quotation> repository, IUnitOfWork unitOfWork)
    : ICommandHandler<AcceptQuotationCommand>
{
    public Task<Result> Handle(AcceptQuotationCommand request, CancellationToken cancellationToken) =>
        QuotationLifecycle.Apply(repository, unitOfWork, request.Id, q => q.Accept(), cancellationToken);
}

public sealed class RejectQuotationCommandHandler(IRepository<Quotation> repository, IUnitOfWork unitOfWork)
    : ICommandHandler<RejectQuotationCommand>
{
    public Task<Result> Handle(RejectQuotationCommand request, CancellationToken cancellationToken) =>
        QuotationLifecycle.Apply(repository, unitOfWork, request.Id, q => q.Reject(), cancellationToken);
}

public sealed class CancelQuotationCommandHandler(IRepository<Quotation> repository, IUnitOfWork unitOfWork)
    : ICommandHandler<CancelQuotationCommand>
{
    public Task<Result> Handle(CancelQuotationCommand request, CancellationToken cancellationToken) =>
        QuotationLifecycle.Apply(repository, unitOfWork, request.Id, q => q.Cancel(), cancellationToken);
}

internal static class QuotationLifecycle
{
    public static async Task<Result> Apply(
        IRepository<Quotation> repository,
        IUnitOfWork unitOfWork,
        long id,
        Action<Quotation> action,
        CancellationToken cancellationToken)
    {
        var quotation = await repository.GetByFilterAsync(e => e.Id == id && e.Status != Status.Deleted, "Lines");
        if (quotation is null)
            return new Result(HttpStatusCode.NotFound, [new Error("Quotation not found.")]);

        try
        {
            action(quotation);
            await repository.UpdateAsync(quotation);
            await unitOfWork.SaveChangeAsync(cancellationToken);
            return new Result(HttpStatusCode.OK, null);
        }
        catch (QuotationDomainException ex)
        {
            return new Result(HttpStatusCode.BadRequest, [new Error(ex.Message)]);
        }
    }
}
