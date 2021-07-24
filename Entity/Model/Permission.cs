using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Permission")]
    public class Permission : BaseModel
    {
        public string Key { get; set; }

        public string Name { get; set; }
    }
}