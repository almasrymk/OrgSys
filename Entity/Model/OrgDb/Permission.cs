using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Permission")]
    public class Permission : BaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override long Id { get ; set ; }

        public string Key { get; set; }

        public string Name { get; set; }
    }
}