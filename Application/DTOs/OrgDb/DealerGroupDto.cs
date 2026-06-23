using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class DealerGroupDto : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string Name { get; set; }
    }
}