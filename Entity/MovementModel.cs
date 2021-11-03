using Entity.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    public class MovementModel : BaseModel
    {
        [Required]
        //[DisplayFormat(ApplyFormatInEditMode =true,DataFormatString ="{0:dd MMM yyyy}")]
        public DateTime Date { get; set; }

        [ForeignKey("CreateUser")]
        public long CreateUserId { get; set; }

        public virtual User CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        [ForeignKey("ModifyUser")]
        public long? ModifyUserId { get; set; }

        public virtual User ModifyUser { get; set; }

        public DateTime? ModifyDate { get; set; }

        public long? ShiftId { get; set; }

        public virtual Shift Shift { get; set; }

        public long? BranchId { get; set; }

        public virtual Branch Branch { get; set; }
    }
}