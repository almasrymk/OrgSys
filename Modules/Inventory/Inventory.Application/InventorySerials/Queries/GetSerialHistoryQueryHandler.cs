namespace Inventory.Application.InventorySerials.Queries;

using System.Net;

public sealed record SerialHistoryDto(
    long Id, long ProductId, string SerialNumber, SerialStatus SerialStatus,
    long? CurrentStockId, long? CurrentLocationId, DateTime ReceivedDate, DateTime? IssuedDate);

public sealed record GetSerialHistoryQuery(long ProductId, string? SerialNumber) : ICommandCollection<SerialHistoryDto>;

public sealed class GetSerialHistoryQueryHandler(IRepository<InventorySerial> serialRepository)
    : ICommandCollectionHandler<GetSerialHistoryQuery, SerialHistoryDto>
{
    public async Task<ResultCollection<SerialHistoryDto>> Handle(GetSerialHistoryQuery request, CancellationToken cancellationToken)
    {
        var serials = (await serialRepository.GetListByFilterAsync(s =>
            s.ProductId == request.ProductId
            && (request.SerialNumber == null || s.SerialNumber == request.SerialNumber)))?.ToList() ?? [];

        var dtos = serials.Select(s => new SerialHistoryDto(
            s.Id, s.ProductId, s.SerialNumber, s.SerialStatus,
            s.CurrentStockId, s.CurrentLocationId, s.ReceivedDate, s.IssuedDate)).ToList();

        return new ResultCollection<SerialHistoryDto>(HttpStatusCode.OK, dtos, null);
    }
}
