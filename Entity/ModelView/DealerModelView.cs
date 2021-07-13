using Utility;
using Entity.Model;
using Utility.Resource;
using System.ComponentModel.DataAnnotations;

namespace Entity.ModelView
{
    public class DealerModelView : BaseModel
    {
        public DealerModelView() 
        { 

        }

        public DealerModelView(Dealer ob)
        {
            if (ob == null) 
                ob = new Dealer();
            this.Id = ob.Id;
            this.CodeNumber = ob.CodeNumber;
            this.Code = ob.Code;
            this.Name = ob.Name;
            this.Phone = ob.Phone;
            this.Email = ob.Email;
            this.Address = ob.Address;
            this.MaskText = ob.MaskText;
            this.Status = ob.Status;
            this.TypeId = ob.TypeId;
            this.ParentId = ob.ParentId;
            this.ImgPath = ob.ImgPath;
        }

        public Dealer Model
        {
            get
            {
                return new Dealer
                {
                    Id = this.Id,
                    CodeNumber = this.CodeNumber,
                    Code = this.Code,
                    Name = this.Name,
                    Phone = this.Phone,
                    Email = this.Email,
                    Address = this.Address,
                    MaskText = this.MaskText,
                    Status = this.Status,
                    TypeId = this.TypeId,
                    ParentId = this.ParentId,
                    ImgPath = this.ImgPath
                };
            }
        }
      
        public long CodeNumber { get; set; }

        [Required(ErrorMessageResourceName = nameof(Message_Designer.CodeRequired), ErrorMessageResourceType = typeof(Message_Designer))]
        public string Code { get; set; }

        [Display(Name = nameof(Title_Designer.Name), ResourceType = typeof(Title_Designer))]
        [Required(ErrorMessageResourceName = nameof(Message_Designer.NameRequired), ErrorMessageResourceType = typeof(Message_Designer))]
        public string Name { get; set; }

        [StringLength(25, MinimumLength = 8, ErrorMessageResourceName = nameof(Message_Designer.PhoneRange), ErrorMessageResourceType = typeof(Message_Designer))]        
        public string Phone { get; set; }

        [StringLength(20, MinimumLength =3, ErrorMessageResourceName = nameof(Message_Designer.EmailRange), ErrorMessageResourceType = typeof(Message_Designer))]
        [EmailAddress(ErrorMessageResourceName = nameof(Message_Designer.EmailFromate), ErrorMessageResourceType = typeof(Message_Designer))]
        public string Email { get; set; }

        [StringLength(500, MinimumLength = 3, ErrorMessageResourceName = nameof(Message_Designer.AddressRange), ErrorMessageResourceType = typeof(Message_Designer))]
        public string Address { get; set; }        
    }
}