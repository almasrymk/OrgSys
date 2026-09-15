namespace Inventory.Domain.Tests;

public class InventoryBatchTests
{
    private static readonly DateTime Today = new(2026, 9, 1);

    [Fact]
    public void Cannot_create_batch_without_number() =>
        Assert.Throws<InvalidBatchException>(() => InventoryBatch.Create(1, "", null, null, null));

    [Fact]
    public void Cannot_create_batch_with_expiry_before_manufacturing() =>
        Assert.Throws<InvalidBatchException>(() =>
            InventoryBatch.Create(1, "B-1", manufacturingDate: Today, expiryDate: Today.AddDays(-1), null));

    // ----- Expired batch cannot be issued -----

    [Fact]
    public void Expired_batch_cannot_be_issued_without_override()
    {
        var batch = InventoryBatch.Create(1, "B-1", Today.AddYears(-1), Today.AddDays(-1), null);

        Assert.Throws<BatchExpiredException>(() => batch.EnsureIssuable(Today, allowExpiredOverride: false));
    }

    [Fact]
    public void Expired_batch_can_be_issued_with_explicit_override()
    {
        var batch = InventoryBatch.Create(1, "B-1", Today.AddYears(-1), Today.AddDays(-1), null);

        batch.EnsureIssuable(Today, allowExpiredOverride: true); // does not throw
    }

    [Fact]
    public void Non_expired_batch_is_issuable()
    {
        var batch = InventoryBatch.Create(1, "B-1", Today, Today.AddDays(30), null);
        batch.EnsureIssuable(Today, allowExpiredOverride: false); // does not throw
    }

    [Fact]
    public void Quarantined_batch_cannot_be_issued_even_if_not_expired()
    {
        var batch = InventoryBatch.Create(1, "B-1", Today, Today.AddDays(30), null);
        batch.Quarantine();

        Assert.Throws<InvalidBatchException>(() => batch.EnsureIssuable(Today, allowExpiredOverride: false));
    }

    [Fact]
    public void RefreshExpiryStatus_marks_active_batch_expired_once_past_expiry_date()
    {
        var batch = InventoryBatch.Create(1, "B-1", Today.AddDays(-10), Today.AddDays(-1), null);
        Assert.Equal(BatchStatus.Active, batch.BatchStatus);

        batch.RefreshExpiryStatus(Today);

        Assert.Equal(BatchStatus.Expired, batch.BatchStatus);
    }
}
