using Domain.Entities;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class FiscalPeriodDto : BaseModel
    {
        public long FiscalYearId { get; set; }

        public int PeriodNumber { get; set; }

        [StringLength(50, MinimumLength = 2)]
        public string? Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public FiscalPeriodStatus FiscalPeriodStatus { get; set; }
    }
}
