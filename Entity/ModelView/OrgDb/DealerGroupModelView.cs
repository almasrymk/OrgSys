using Entity.Model;
using System.ComponentModel.DataAnnotations;

namespace Entity.ModelView
{
    public class DealerGroupModelView : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string Name { get; set; }
    }
}