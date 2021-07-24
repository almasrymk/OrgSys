using Entity.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace Entity
{
    public class MovementModel : BaseModel
    {
        [Required]
        public DateTime Date { get; set; }

        public long CreateUserId { get; set; }

        public virtual User CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        public long? ModifyUserId { get; set; }

        public virtual User ModifyUser { get; set; }

        public DateTime? ModifyDate { get; set; }

        public long? ShiftId { get; set; }

        public virtual Shift Shift { get; set; }

        public long? BranchId { get; set; }

        public virtual Branch Branch { get; set; }
    }
}