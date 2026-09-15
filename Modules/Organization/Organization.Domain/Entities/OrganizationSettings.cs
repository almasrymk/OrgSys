namespace Organization.Domain
{
    /// <summary>
    /// One typed settings row per Company — display/default configuration only (default currency,
    /// default country, default timezone, a fiscal-calendar display convention). Deliberately NOT a
    /// generic key/value store (that's Administration.Domain.Preference, which stays user-scoped and
    /// cross-cutting-technical) and NOT a place for other bounded contexts' business configuration
    /// (Inventory costing method, Sales approval policy, Tax rules, GL posting rules all stay in
    /// their own modules). See docs/organization/organization-target-architecture.md §4.
    /// </summary>
    [Table("OrganizationSettings")]
    public class OrganizationSettings : BaseModel
    {
        [Required]
        public virtual long CompanyId { get; set; }

        public virtual Company? Company { get; set; }

        /// <summary>Scalar-only reference into MasterData.Domain.Currency — see Company.DefaultCurrencyId.</summary>
        public virtual long? DefaultCurrencyId { get; set; }

        /// <summary>Scalar-only reference into MasterData.Domain.Country — see Company.CountryId.</summary>
        public virtual long? DefaultCountryId { get; set; }

        [StringLength(100)]
        public virtual string? DefaultTimeZone { get; set; }

        /// <summary>Display/default convention only — does NOT control Accounting.Domain.FiscalYear's
        /// own authoritative StartDate/EndDate for any given fiscal year; it only pre-fills a
        /// suggested start month/day when a new fiscal year is created in Accounting.</summary>
        [Range(1, 12)]
        public virtual int? FiscalYearStartMonth { get; set; }

        [Range(1, 31)]
        public virtual int? FiscalYearStartDay { get; set; }
    }
}
