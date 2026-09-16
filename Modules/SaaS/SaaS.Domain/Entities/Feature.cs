namespace SaaS.Domain
{
    /// <summary>A gate-able module/capability (brief §54) — e.g. "GeneralLedger", "Budgeting",
    /// "AdvancedReporting", "MultiBranch", "APIAccess". Key follows the same stable, client-assigned
    /// identifier convention as Administration.Domain.Permission.Key (seeded with fixed values,
    /// referenced by string from other modules' feature checks, never renamed once shipped).</summary>
    [Table("Feature")]
    public class Feature : BaseModel
    {
        [Required]
        [StringLength(100)]
        public virtual string Key { get; set; } = null!;

        [Required]
        [StringLength(150)]
        public virtual string Name { get; set; } = null!;

        [StringLength(500)]
        public virtual string? Description { get; set; }
    }
}
