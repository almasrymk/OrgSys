namespace Inventory.Domain.Tests;

public class StockReservationTests
{
    private static readonly DateTime Date = new(2026, 9, 1);

    private static StockReservation NewReservation(decimal quantity = 10) => StockReservation.Create(
        productId: 1, stockId: 1, locationId: null, batchId: null, serialId: null,
        quantity: quantity, sourceType: SourceDocumentType.SalesOrder, sourceId: 500, sourceLineId: 1,
        createUserId: 1, createDate: Date, expiresAt: null);

    [Fact]
    public void Create_raises_stock_reserved_event()
    {
        var reservation = NewReservation(10);
        var evt = Assert.Single(reservation.DomainEvents);
        Assert.IsType<StockReservedDomainEvent>(evt);
        Assert.Equal(ReservationStatus.Active, reservation.ReservationStatus);
    }

    [Fact]
    public void Cannot_create_reservation_with_zero_quantity() =>
        Assert.Throws<InsufficientStockException>(() => NewReservation(0));

    [Fact]
    public void Release_transitions_to_released()
    {
        var reservation = NewReservation();
        reservation.Release(Date);

        Assert.Equal(ReservationStatus.Released, reservation.ReservationStatus);
        Assert.NotNull(reservation.ReleasedAt);
    }

    [Fact]
    public void Fulfill_transitions_to_fulfilled()
    {
        var reservation = NewReservation();
        reservation.Fulfill(Date);

        Assert.Equal(ReservationStatus.Fulfilled, reservation.ReservationStatus);
        Assert.NotNull(reservation.FulfilledAt);
    }

    [Fact]
    public void Cannot_release_an_already_fulfilled_reservation()
    {
        var reservation = NewReservation();
        reservation.Fulfill(Date);

        Assert.Throws<ReservationAlreadyFulfilledException>(() => reservation.Release(Date));
    }

    [Fact]
    public void Expired_check_only_true_while_active_and_past_expiry()
    {
        var reservation = StockReservation.Create(
            1, 1, null, null, null, 5, SourceDocumentType.SalesOrder, 500, null,
            1, Date, expiresAt: Date.AddDays(1));

        Assert.False(reservation.IsExpired(Date));
        Assert.True(reservation.IsExpired(Date.AddDays(2)));

        reservation.Fulfill(Date.AddDays(2));
        Assert.False(reservation.IsExpired(Date.AddDays(3))); // no longer Active
    }
}
