using System.ComponentModel.DataAnnotations;

namespace Entity.ModelView
{
    public class BankModelView : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string Name { get; set; }
    }
}