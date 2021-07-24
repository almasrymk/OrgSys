using System.ComponentModel.DataAnnotations;
using Utility;

namespace Entity
{
    public class BaseModel
    {      
        public virtual long Id { get; set; }

        public virtual long CodeNumber { get; set; }

        public virtual string Code { get; set; }

        public string MaskText { get; set; }

        public long ParentId { get; set; }

        public long TypeId { get; set; }

        public bool Hide { get; set; }

        public string ImgPath { get; set; }

        public Status Status { get; set; }        
    }
}