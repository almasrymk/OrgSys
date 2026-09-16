namespace Advances.Application.Custodies.Queries;

using System.Net;

public sealed record GetCustodyByIdQuery(long Id) : IQuery<CustodyDto>;

public sealed class GetCustodyByIdQueryHandler(IRepository<Custody> repository)
    : IQueryHandler<GetCustodyByIdQuery, CustodyDto>
{
    public async Task<Result<CustodyDto>> Handle(GetCustodyByIdQuery request, CancellationToken cancellationToken)
    {
        var custody = await repository.GetByFilterAsync(
            e => e.Id == request.Id && e.Status != Status.Deleted,
            "Handovers");
        if (custody is null)
            return new Result<CustodyDto>(HttpStatusCode.NotFound, null, [new Error("Custody not found.")]);

        return new Result<CustodyDto>(HttpStatusCode.OK, ToDto(custody), null);
    }

    internal static CustodyDto ToDto(Custody custody) => new(
        custody.Id,
        custody.Code,
        custody.HolderId,
        custody.Purpose,
        custody.DueDate,
        custody.CurrencyId,
        custody.Rate,
        custody.IssuedAmount,
        custody.SettledAmount,
        custody.ReturnedAmount,
        custody.OutstandingAmount,
        custody.LifecycleStatus,
        custody.IssueDate,
        custody.IssuingFinancialTransactionId,
        custody.ReturnFinancialTransactionId,
        custody.Notes,
        custody.Date,
        custody.BranchId,
        custody.Handovers.Select(h => new CustodyHandoverDto(
            h.Id, h.FromHolderId, h.ToHolderId, h.TransferDate, h.TransferredAmount, h.Reason, h.ApprovedByUserId)).ToList());
}
