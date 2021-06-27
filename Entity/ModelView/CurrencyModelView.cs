using Utility;
using Entity.Model;
using Utility.Resource;
using System.ComponentModel.DataAnnotations;

namespace Entity.ModelView
{
    public class CurrencyModelView : BaseModel
    {
        public CurrencyModelView()
        {

        }

        public CurrencyModelView(Currency ob)
        {
            if (ob == null)
                ob = new Currency();
            this.Id = ob.Id;
            this.Name = ob.Name;
            this.Rate = ob.Rate;
            this.IsDefault = ob.IsDefault;
            this.Status = ob.Status;
            this.ParentId = ob.ParentId;
            this.TypeId = ob.TypeId;
            this.ImgPath = ob.ImgPath;
        }

        public Currency Model
        {
            get
            {
                return new Currency
                {
                    Id = this.Id,
                    Name = this.Name,
                    Rate = this.Rate,
                    IsDefault = this.IsDefault,
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
        public decimal Rate { get; set; }
        public bool IsDefault { get; set; }
    }
}