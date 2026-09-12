using System.ComponentModel.DataAnnotations;

namespace Parties.Application
{
    public class DealerDto : BaseModel
    {
        public virtual string? Name { get; set; }

        [StringLength(25, MinimumLength = 8)]
        public virtual string? Phone { get; set; }

        [StringLength(30, MinimumLength = 3)]
        public virtual string? Email { get; set; }

        [StringLength(500, MinimumLength = 3)]
        public virtual string? Address { get; set; }

        public virtual long? DealerGroupId { get; set; }

        public virtual string? DealerGroupName { get; set; }

        public virtual long? CountryId { get; set; }

        public virtual string? CountryName { get; set; }

        public virtual long? CityId { get; set; }

        public virtual string? CityName { get; set; }

        public virtual long? DistrictId { get; set; }

        public virtual string? DistrictName { get; set; }
        public virtual long? AccountId { get; set; }

        public virtual string? AccountCode { get; set; }

        public virtual string? AccountName { get; set; }

        /// <summary>UI-only convenience flag — when true and no <see cref="AccountId"/> is given, a
        /// customer sub-account is auto-created under the configured AR parent account and linked.
        /// Not persisted on the Dealer entity itself.</summary>
        public virtual bool? AutoCreateReceivableAccount { get; set; }

        /// <summary>UI-only convenience flag — when true and no <see cref="AccountId"/> is given, a
        /// supplier sub-account is auto-created under the configured AP parent account and linked.
        /// Not persisted on the Dealer entity itself.</summary>
        public virtual bool? AutoCreatePayableAccount { get; set; }
    }
}