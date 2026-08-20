using Domain.Entities;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class FiscalYearDto : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public string? Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsCurrent { get; set; }

        public FiscalYearStatus FiscalYearStatus { get; set; }

        public ICollection<FiscalPeriodDto>? Periods { get; set; }
    }
}
