namespace Organization.Application
{
    using System.ComponentModel.DataAnnotations;

    public class OrganizationSettingsDto : BaseModel
    {
        public long CompanyId { get; set; }

        public long? DefaultCurrencyId { get; set; }

        public long? DefaultCountryId { get; set; }

        [StringLength(100)]
        public string? DefaultTimeZone { get; set; }

        [Range(1, 12)]
        public int? FiscalYearStartMonth { get; set; }

        [Range(1, 31)]
        public int? FiscalYearStartDay { get; set; }
    }
}
