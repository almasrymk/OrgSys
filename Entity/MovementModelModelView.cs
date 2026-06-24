using System;

namespace Entity
{
    public class MovementModelView : BaseModel
    {        
        public DateTime Date { get; set; }

        public long CreateUserId { get; set; }

        public DateTime CreateDate { get; set; }

        public long? ModifyUserId { get; set; }

        public DateTime? ModifyDate { get; set; }

        public long? ShiftId { get; set; }

        public long? BranchId { get; set; }

        public bool HasJournal { get; set; }

        public bool Review { get; set; }

        public bool  Posted { get; set; }
    }
}