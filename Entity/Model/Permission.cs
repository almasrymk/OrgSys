using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Utility;

namespace Entity.Model
{
    [Table("Permission")]
    public class Permission : BaseModel
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Name2 { get; set; }
    }
}
