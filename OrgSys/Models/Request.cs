using System;
using System.ComponentModel.DataAnnotations;

namespace OrgSys.Models
{
    public class Request : BaseModel
    {
        public virtual string? Name { get; set; }
        public virtual string? Email { get; set; }
        public virtual string? Phone { get; set; }
        public virtual string? CompanyName { get; set; }
        public virtual string? URL { get; set; }
        public virtual string? Key { get; set; }

        [Required]
        public virtual DateTime ExpireDate { get; set; } = default!;
    }
}
