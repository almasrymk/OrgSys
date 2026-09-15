namespace Parties.Application
{
    using System.ComponentModel.DataAnnotations;

    public class PartyContactDto : BaseModel
    {
        public virtual long DealerId { get; set; }

        [StringLength(150, MinimumLength = 2)]
        public virtual string? Name { get; set; }

        [StringLength(100)]
        public virtual string? JobTitle { get; set; }

        [StringLength(100)]
        public virtual string? Department { get; set; }

        [StringLength(30, MinimumLength = 3)]
        public virtual string? Email { get; set; }

        [StringLength(25, MinimumLength = 8)]
        public virtual string? Phone { get; set; }

        [StringLength(25, MinimumLength = 8)]
        public virtual string? Mobile { get; set; }

        public virtual bool IsPrimary { get; set; }

        public virtual bool IsActive { get; set; }
    }
}
