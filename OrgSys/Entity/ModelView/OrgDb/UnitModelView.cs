using System.ComponentModel.DataAnnotations;

namespace Entity.ModelView
{
    public class UnitModelView : BaseModel
    {
        [StringLength(50, MinimumLength = 2)]
        public virtual string Name { get; set; }
    }
}