namespace Parties.Application
{
    using System.ComponentModel.DataAnnotations;
    using Parties.Domain;

    public class PartyAddressDto : BaseModel
    {
        public virtual long DealerId { get; set; }

        public virtual PartyAddressType AddressType { get; set; }

        [StringLength(500, MinimumLength = 3)]
        public virtual string? Line1 { get; set; }

        [StringLength(500)]
        public virtual string? Line2 { get; set; }

        public virtual long? CountryId { get; set; }

        public virtual string? CountryName { get; set; }

        public virtual long? CityId { get; set; }

        public virtual string? CityName { get; set; }

        public virtual long? DistrictId { get; set; }

        public virtual string? DistrictName { get; set; }

        [StringLength(20)]
        public virtual string? PostalCode { get; set; }

        public virtual bool IsPrimary { get; set; }
    }
}
