namespace Inventory.Domain.Tests;

public class InventorySerialTests
{
    private static readonly DateTime Date = new(2026, 9, 1);

    private static InventorySerial NewSerial() =>
        InventorySerial.Receive(productId: 1, serialNumber: "SN-001", stockId: 1, locationId: null, batchId: null, receivedDate: Date);

    [Fact]
    public void Receive_creates_available_serial()
    {
        var serial = NewSerial();
        Assert.Equal(SerialStatus.Available, serial.SerialStatus);
        Assert.IsType<SerialReceivedDomainEvent>(Assert.Single(serial.DomainEvents));
    }

    [Fact]
    public void Cannot_receive_serial_without_number() =>
        Assert.Throws<SerialNotAvailableException>(() => InventorySerial.Receive(1, "", 1, null, null, Date));

    // ----- Serial cannot be issued twice -----

    [Fact]
    public void Mark_issued_transitions_to_issued()
    {
        var serial = NewSerial();
        serial.MarkIssued(Date);

        Assert.Equal(SerialStatus.Issued, serial.SerialStatus);
        Assert.Null(serial.CurrentStockId);
    }

    [Fact]
    public void Cannot_issue_an_already_issued_serial()
    {
        var serial = NewSerial();
        serial.MarkIssued(Date);

        Assert.Throws<SerialAlreadyIssuedException>(() => serial.MarkIssued(Date));
    }

    [Fact]
    public void Reserved_serial_can_still_be_issued_once()
    {
        var serial = NewSerial();
        serial.Reserve();

        serial.MarkIssued(Date);

        Assert.Equal(SerialStatus.Issued, serial.SerialStatus);
    }

    [Fact]
    public void Returned_serial_reactivates_at_a_location()
    {
        var serial = NewSerial();
        serial.MarkIssued(Date);

        serial.MarkReturned(stockId: 2, locationId: null);

        Assert.Equal(SerialStatus.Returned, serial.SerialStatus);
        Assert.Equal(2, serial.CurrentStockId);
    }

    [Fact]
    public void Cannot_move_an_issued_serial()
    {
        var serial = NewSerial();
        serial.MarkIssued(Date);

        Assert.Throws<SerialNotAvailableException>(() => serial.MoveTo(2, null));
    }
}
