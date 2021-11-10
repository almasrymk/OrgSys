using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Plan", Schema = "admin")]
    public class Plan : BaseModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string Offer { get; set; }
        public string PriceAfterOffer { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public long PlanTypeId { get; set; }
        public virtual PlanType PlanType { get; set; }
        public virtual List<PlanElement> PlanElements { get; set; }
    }
}