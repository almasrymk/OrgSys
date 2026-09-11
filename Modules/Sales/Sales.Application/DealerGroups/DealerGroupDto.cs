using System.ComponentModel.DataAnnotations;

namespace Sales.Application
{
    public class DealerGroupDto : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string? Name { get; set; }
    }
}