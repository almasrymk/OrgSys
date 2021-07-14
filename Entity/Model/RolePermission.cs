using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Utility;

namespace Entity.Model
{
    [Table("RolePermission")]
    public class RolePermission : BaseModel
    {
        public long RoleId { get; set; }
        public long PermissionId { get; set; }
        public virtual Role Role { get; set; }
        public virtual Permission Permission { get; set; }
    }
}
