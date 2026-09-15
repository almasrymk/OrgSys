namespace Catalog.Domain
{
    [Table("Property")]
   public class Property : BaseModel
    {       
        [StringLength(50, MinimumLength = 3)]
        public virtual string? Name { get; set; }

        /// <summary>Additive (Catalog Phase 7) — the value shape this attribute expects. Defaults
        /// to Selection, preserving every existing row's current implicit behavior unchanged.</summary>
        public virtual AttributeDataType DataType { get; set; } = AttributeDataType.Selection;

        public virtual ICollection<PropertyElement>? PropertyElements { get; set; }
    }
}