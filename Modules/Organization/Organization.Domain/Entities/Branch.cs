namespace Organization.Domain
{
    [Table("Branch")]
    public class Branch : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public string? Name { get; set; }

        /// <summary>Required — a Branch must belong to a Company (brief §1.4). Real EF navigation:
        /// Company and Branch are both Organization.Domain, so this is an intra-module reference,
        /// not a cross-module one. Backfilled for existing live Branch rows by the
        /// AddOrganizationCompanyAndSettings migration itself (not the seeder — the seeder only
        /// covers a fresh database).</summary>
        [ForeignKey("Company")]
        public virtual long CompanyId { get; set; }

        public virtual Company? Company { get; set; }
    }
}