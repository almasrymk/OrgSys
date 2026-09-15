namespace Catalog.Domain
{
    /// <summary>
    /// New Catalog-owned master data (docs/catalog/catalog-target-architecture.md §2) — did not
    /// exist anywhere in OrgSys before this module. Follows the same flat, plain-property shape as
    /// the relocated Classification/Unit rather than the newer AggregateRoot/factory-method style,
    /// for consistency with the other Catalog master-data entities it sits alongside.
    /// </summary>
    [Table("Brand")]
    public class Brand : BaseModel
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public virtual string Name { get; set; } = null!;

        public virtual string? Description { get; set; }

        public virtual bool IsActive { get; set; } = true;
    }
}
