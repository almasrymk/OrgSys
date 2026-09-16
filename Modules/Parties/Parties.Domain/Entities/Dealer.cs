namespace Parties.Domain
{
    [Table("Dealer")]
    public class Dealer : BaseModel
    {
        [Required]
        public virtual string Name { get; set; } = null!;

        [StringLength(25, MinimumLength = 8)] 
        public virtual string? Phone { get; set; }

        [StringLength(30, MinimumLength = 3)]
        public virtual string? Email { get; set; }

        [StringLength(500, MinimumLength = 3)]
        public virtual string? Address { get; set; }

        /// <summary>Person vs Organization (brief §2.2) — nullable, additive; not backfilled for
        /// existing rows (see docs/parties/party-target-architecture.md §1).</summary>
        public virtual PartyType? PartyType { get; set; }

        [StringLength(50)]
        public virtual string? TaxRegistrationNumber { get; set; }

        [StringLength(50)]
        public virtual string? CommercialRegistrationNumber { get; set; }

        /// <summary>A Dealer may hold a Customer role, a Supplier role, or both simultaneously
        /// (brief §2.9's mandatory scenario) — <see cref="TypeId"/> (inherited from BaseModel,
        /// interpreted as <see cref="DealerType"/>) remains the Dealer's *original/primary* role for
        /// every pre-existing screen, query, and uniqueness rule; these two profiles are the
        /// additive mechanism through which a Dealer gains the *other* role without a duplicate
        /// row. See AssignCustomerRoleCommand/AssignSupplierRoleCommand.</summary>
        public virtual CustomerProfile? CustomerProfile { get; set; }

        public virtual SupplierProfile? SupplierProfile { get; set; }

        public virtual ICollection<PartyContact> Contacts { get; set; } = new List<PartyContact>();

        public virtual ICollection<PartyAddress> Addresses { get; set; } = new List<PartyAddress>();

        [ForeignKey("DealerGroup")]
        public virtual long? DealerGroupId { get; set; }

        public virtual DealerGroup? DealerGroup { get; set; }

        /// <summary>Scalar-only MasterData geo references. FKs preserved in OrgContext.</summary>
        public virtual long? CountryId { get; set; }

        public virtual long? CityId { get; set; }

        public virtual long? DistrictId { get; set; }

        // No navigation to Accounting.Domain.Account — GeneralLedger bounded-context isolation
        // forbids Parties.Domain from referencing Accounting.Domain (see the GeneralLedger
        // migration report). The FK column/constraint is preserved via a Fluent "no navigation"
        // relationship in OrgContext.OnModelCreating, same pattern as MovementModel's
        // CreateUser/ModifyUser/Shift/Branch. Application code that needs the linked Account
        // (e.g. DealerMappingProfile's AccountCode) resolves it via Accounting.Domain.Account
        // directly at the Application layer, which already has an accepted cross-module
        // dependency on Accounting.Domain (see ModuleLayerDependencyTests).
        public virtual long? AccountId { get; set; }
    }
}