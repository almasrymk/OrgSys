using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class UnitDto : BaseModel
    {
        [StringLength(50, MinimumLength = 2)]
        public virtual string Name { get; set; }
    }
}