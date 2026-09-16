namespace Organization.Domain
{
    [Table("Company")]
    public class Company : BaseModel
    {
        /// <summary>Scalar-only reference into SaaS.Domain.Tenant — deliberately no EF navigation,
        /// same convention as DefaultCurrencyId/CountryId below. Nullable: this is stage 1 of the
        /// staged multi-tenant retrofit (docs/architecture/adr/tenant-vs-company.md) — existing rows
        /// are backfilled to the seeded Default Tenant by OrganizationDataSeeder/a data migration
        /// before this column is ever made required.</summary>
        public virtual long? TenantId { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 3)]
        public virtual string LegalName { get; set; } = null!;

        [StringLength(150)]
        public virtual string? TradeName { get; set; }

        [StringLength(50)]
        public virtual string? TaxRegistrationNumber { get; set; }

        [StringLength(50)]
        public virtual string? CommercialRegistrationNumber { get; set; }

        /// <summary>Scalar-only reference into MasterData.Domain.Currency — deliberately no EF
        /// navigation. Currency/Country ownership stays in MasterData this pass (see
        /// docs/organization/organization-target-architecture.md); wired via a Fluent "no
        /// navigation" FK in OrgContext, same convention as Journal.CurrencyId.</summary>
        public virtual long? DefaultCurrencyId { get; set; }

        /// <summary>Scalar-only reference into MasterData.Domain.Country — see DefaultCurrencyId.</summary>
        public virtual long? CountryId { get; set; }

        [StringLength(500)]
        public virtual string? Address { get; set; }

        [StringLength(25, MinimumLength = 8)]
        public virtual string? Phone { get; set; }

        [StringLength(100)]
        public virtual string? Email { get; set; }

        public virtual string? Website { get; set; }

        public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();
    }
}
