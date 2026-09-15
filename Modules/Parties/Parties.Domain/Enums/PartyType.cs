namespace Parties.Domain
{
    /// <summary>
    /// Person vs Organization — distinct from <see cref="DealerType"/> (Client/Supplier), which is
    /// a role, not an identity type (brief §2.2: "do NOT use CustomerType/SupplierType to describe
    /// whether something is a person/company — those are roles, not identity types"). Nullable on
    /// Dealer since every pre-existing row predates this field; not backfilled — see
    /// docs/parties/party-target-architecture.md.
    /// </summary>
    public enum PartyType
    {
        Person = 1,
        Organization = 2
    }
}
