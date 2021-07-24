using Entity.Model;

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

            this.Name = ob.Name;

            this.Phone = ob.Phone;

            this.Email = ob.Email;

            this.Address = ob.Address;

            this.Id = ob.Id;

            this.CodeNumber = ob.CodeNumber;

            this.Code = ob.Code;

            this.MaskText = ob.MaskText;

            this.ParentId = ob.ParentId;

            this.TypeId = ob.TypeId;

            this.Hide = ob.Hide;

            this.ImgPath = ob.ImgPath;

            this.Status = ob.Status;
        }

        public Dealer Model
        {
            get
            {
                return new Dealer
                {
                    Name = this.Name,
                    Phone = this.Phone,
                    Email = this.Email,
                    Address = this.Address,
                    Id = this.Id,
                    CodeNumber = this.CodeNumber,
                    Code = this.Code,
                    MaskText = this.MaskText,
                    ParentId = this.ParentId,
                    TypeId = this.TypeId,
                    Hide = this.Hide,
                    Status = this.Status,
                    ImgPath = this.ImgPath
                };
            }
        }           

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }        
    }
}