using System.ComponentModel.DataAnnotations;

namespace Catalog.Application
{
    public class UnitDto : BaseModel
    {
        [StringLength(50, MinimumLength = 2)]
        public virtual string? Name { get; set; }
    }
}