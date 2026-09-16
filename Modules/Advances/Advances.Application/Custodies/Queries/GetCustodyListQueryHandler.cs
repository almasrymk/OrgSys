namespace Advances.Application.Custodies.Queries;

using System.Net;

public sealed record GetCustodyListQuery(long? HolderId, CustodyStatus? Status) : ICommandCollection<CustodyDto>;

public sealed class GetCustodyListQueryHandler(IRepository<Custody> repository)
    : ICommandCollectionHandler<GetCustodyListQuery, CustodyDto>
{
    public async Task<ResultCollection<CustodyDto>> Handle(GetCustodyListQuery request, CancellationToken cancellationToken)
    {
        var rows = (await repository.GetListByFilterAsync(e =>
            e.Status != Status.Deleted
            && (request.HolderId == null || e.HolderId == request.HolderId)
            && (request.Status == null || e.LifecycleStatus == request.Status),
            "Handovers"))?.OrderByDescending(e => e.Id).ToList() ?? [];

        return new ResultCollection<CustodyDto>(
            HttpStatusCode.OK,
            rows.Select(GetCustodyByIdQueryHandler.ToDto).ToList(),
            null);
    }
}
