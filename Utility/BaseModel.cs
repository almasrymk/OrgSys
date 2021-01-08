using System;
using System.Collections.Generic;
using System.Text;

namespace Utility
{
    public class BaseModel
    {      
        public long Id { get; set; }
        public string MaskText { get; set; }
        public long ParentId { get; set; }
        public long TypeId { get; set; }
        public string ImgPath { get; set; }
        public Status Status { get; set; }
    }
}
