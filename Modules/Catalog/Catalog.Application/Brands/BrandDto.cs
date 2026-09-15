using System.ComponentModel.DataAnnotations;

namespace Catalog.Application
{
    public class BrandDto : BaseModel
    {
        [Required]
        public virtual string? Name { get; set; }

        public virtual string? Description { get; set; }

        public virtual bool IsActive { get; set; } = true;
    }
}
