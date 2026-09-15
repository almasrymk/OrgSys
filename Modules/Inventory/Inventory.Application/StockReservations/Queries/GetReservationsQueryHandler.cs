namespace Inventory.Application.StockReservations.Queries;

using System.Net;

public sealed record ReservationDto(
    long Id, long ProductId, long StockId, long? LocationId, long? BatchId, long? SerialId,
    decimal Quantity, SourceDocumentType SourceType, long SourceId, long? SourceLineId,
    ReservationStatus ReservationStatus, DateTime? ExpiresAt, DateTime CreateDate);

public sealed record GetReservationsQuery(long? ProductId, long? StockId, ReservationStatus? Status) : ICommandCollection<ReservationDto>;

public sealed class GetReservationsQueryHandler(IRepository<StockReservation> reservationRepository)
    : ICommandCollectionHandler<GetReservationsQuery, ReservationDto>
{
    public async Task<ResultCollection<ReservationDto>> Handle(GetReservationsQuery request, CancellationToken cancellationToken)
    {
        var reservations = (await reservationRepository.GetListByFilterAsync(r =>
            (request.ProductId == null || r.ProductId == request.ProductId)
            && (request.StockId == null || r.StockId == request.StockId)
            && (request.Status == null || r.ReservationStatus == request.Status)))?.ToList() ?? [];

        var dtos = reservations.Select(r => new ReservationDto(
            r.Id, r.ProductId, r.StockId, r.LocationId, r.BatchId, r.SerialId,
            r.Quantity, r.SourceType, r.SourceId, r.SourceLineId,
            r.ReservationStatus, r.ExpiresAt, r.CreateDate)).ToList();

        return new ResultCollection<ReservationDto>(HttpStatusCode.OK, dtos, null);
    }
}
