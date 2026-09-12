using System.ComponentModel.DataAnnotations;

namespace Parties.Application
{
    public class DealerGroupDto : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string? Name { get; set; }
    }
}