namespace Parties.Contracts.Dealers;

/// <summary>
/// Public contract mirror of Parties.Domain.DealerType — a Dealer's role (Customer/Supplier).
/// Kept as a separate Contracts-owned enum (same underlying values) so consumers never need a
/// Parties.Domain reference just to compare a role. Client = 1, Supplier = 2.
/// </summary>
public enum DealerType
{
    Client = 1,
    Supplier = 2
}
