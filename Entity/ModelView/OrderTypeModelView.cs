using Utility;
using Entity.Model;
using Utility.Resource;
using System.ComponentModel.DataAnnotations;

namespace Entity.ModelView
{
    public class OrderTypeModelView : BaseModel
    {
        public OrderTypeModelView()
        {

        }

        public OrderTypeModelView(OrderType ob)
        {
            if (ob == null)
                ob = new OrderType();
            this.Id = ob.Id;
            this.Name = ob.Name;
            this.Status = ob.Status;
            this.Icon = ob.Icon;
            this.ParentId = ob.ParentId;
            this.TypeId = ob.TypeId;
            this.ImgPath = ob.ImgPath;
        }

        public OrderType Model
        {
            get
            {
                return new OrderType
                {
                    Id = this.Id,
                    Name = this.Name,
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
        public string Icon { get; set; }
    }
}