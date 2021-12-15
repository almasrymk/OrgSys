using System;
using Utility;
using Entity.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    public class MovementModel : BaseModel
    {       
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

        public bool HasJournal { get; set; }

        public bool Review { get; set; }

        public bool  Posted { get; set; }
    }
}