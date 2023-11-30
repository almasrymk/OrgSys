using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("ClientPlan", Schema = "admin")]
    public class ClientPlan : BaseModel
    {
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
        public virtual long ClientId { get; set; }
        public virtual Client Client { get; set; }
        public virtual long PlanId { get; set; }
        public virtual Plan Plan { get; set; }
    }
}