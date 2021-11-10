using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Preference", Schema = "org")]
    public class Preference : BaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override long Id { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public string Reference { get; set; }

        public long? UserId { get; set; }
    }
}