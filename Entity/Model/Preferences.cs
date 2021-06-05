using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Utility;

namespace Entity.Model
{
    [Table("Preference")]
    public class Preference : BaseModel
    {       
        public string Key { get; set; }
        public string Value { get; set; }
        public string Reference { get; set; }
        public long? UserId { get; set; }
    }
}
