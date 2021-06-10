using Utility;
using Entity.Model;
using Utility.Resource;
using System.ComponentModel.DataAnnotations;

namespace Entity.ModelView
{
    public class InvoiceTypeModelView : BaseModel
    {
        public InvoiceTypeModelView()
        {

        }

        public InvoiceTypeModelView(InvoiceType ob)
        {
            if (ob == null)
                ob = new InvoiceType();
            this.Id = ob.Id;
            this.Name = ob.Name;
            this.Group = ob.Group;
            this.Status = ob.Status;
            this.InOut = ob.InOut;
            this.Icon = ob.Icon;
            this.ParentId = ob.ParentId;
            this.TypeId = ob.TypeId;
            this.ImgPath = ob.ImgPath;
        }

        public InvoiceType Model
        {
            get
            {
                return new InvoiceType
                {
                    Id = this.Id,
                    Name = this.Name,
                    Group = this.Group,
                    InOut = this.InOut,
                    Icon = this.Icon,
                    Status = this.Status,
                    MaskText = this.MaskText,
                    ParentId = this.ParentId,
                    TypeId = this.TypeId,
                    ImgPath = this.ImgPath
                };
            }
        }
      
        public string Name { get; set; }
        public int InOut { get; set; }
        public string Icon { get; set; }
        public string Group { get; set; }
    }
}