using Utility;
using Entity.Model;
using Utility.Resource;
using System.ComponentModel.DataAnnotations;

namespace Entity.ModelView
{
    public class PaymentTypeModelView : BaseModel
    {
        public PaymentTypeModelView()
        {

        }

        public PaymentTypeModelView(PaymentType ob)
        {
            if (ob == null)
                ob = new PaymentType();
            this.Id = ob.Id;
            this.Name = ob.Name;
            this.Status = ob.Status;
            this.ParentId = ob.ParentId;
            this.TypeId = ob.TypeId;
            this.ImgPath = ob.ImgPath;
        }

        public PaymentType Model
        {
            get
            {
                return new PaymentType
                {
                    Id = this.Id,
                    Name = this.Name,
                    Status = this.Status,
                    MaskText = this.MaskText,
                    ParentId = this.ParentId,
                    TypeId = this.TypeId,
                    ImgPath = this.ImgPath
                };
            }
        }

        [Display(Name = nameof(Title_Designer.Name), ResourceType = typeof(Title_Designer))]
        [StringLength(50, MinimumLength = 3, ErrorMessageResourceName = nameof(Message_Designer.NameRequired), ErrorMessageResourceType = typeof(Message_Designer))]
        [Required(ErrorMessageResourceName = nameof(Message_Designer.NameRequired), ErrorMessageResourceType = typeof(Message_Designer))]
        public string Name { get; set; }
    }
}