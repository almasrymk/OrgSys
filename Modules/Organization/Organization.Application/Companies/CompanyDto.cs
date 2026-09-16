namespace Organization.Application
{
    using System.ComponentModel.DataAnnotations;

    public class CompanyDto : BaseModel
    {
        public long? TenantId { get; set; }

        [StringLength(150, MinimumLength = 3)]
        public string? LegalName { get; set; }

        [StringLength(150)]
        public string? TradeName { get; set; }

        [StringLength(50)]
        public string? TaxRegistrationNumber { get; set; }

        [StringLength(50)]
        public string? CommercialRegistrationNumber { get; set; }

        public long? DefaultCurrencyId { get; set; }

        public long? CountryId { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        [StringLength(25, MinimumLength = 8)]
        public string? Phone { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }

        public string? Website { get; set; }
    }
}
