using Utility;
using Entity.Model;
using Utility.Resource;
using System.ComponentModel.DataAnnotations;

namespace Entity.ModelView
{
    public class ClassificationModelView : BaseModel
    {
        public ClassificationModelView()
        {

        }

        public ClassificationModelView(Classification ob)
        {
            if (ob == null)
                ob = new Classification();
            this.Id = ob.Id;
            this.Name = ob.Name;
            this.BePurchased = ob.BePurchased;
            this.BeSold = ob.BeSold;
            this.BeManufactured = ob.BeManufactured;
            this.Status = ob.Status;
            this.ParentId = ob.ParentId;
            this.TypeId = ob.TypeId;
            this.ImgPath = ob.ImgPath;
        }

        public Classification Model
        {
            get
            {
                return new Classification
                {
                    Id = this.Id,
                    Name = this.Name,
                    BePurchased = this.BePurchased,
                    BeSold = this.BeSold,
                    BeManufactured = this.BeManufactured,
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

        [Display(Name = nameof(Title_Designer.BePurchased), ResourceType = typeof(Title_Designer))]
        public bool? BePurchased { get; set; }

        [Display(Name = nameof(Title_Designer.BeSold), ResourceType = typeof(Title_Designer))]
        public bool? BeSold { get; set; }

        [Display(Name = nameof(Title_Designer.BeManufactured), ResourceType = typeof(Title_Designer))]
        public bool? BeManufactured { get; set; }
    }
}