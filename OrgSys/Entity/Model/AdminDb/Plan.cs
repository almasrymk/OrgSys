using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Plan", Schema = "admin")]
    public class Plan : BaseModel
    {
        [Required]
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string Price { get; set; }
        public virtual string Offer { get; set; }
        public virtual string PriceAfterOffer { get; set; }
        public virtual string StartDate { get; set; }
        public virtual string EndDate { get; set; }
        public virtual long PlanTypeId { get; set; }
        public virtual PlanType PlanType { get; set; }
        public virtual List<PlanElement> PlanElements { get; set; }
    }
}