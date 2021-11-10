using Entity.Model;

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

            this.Name = ob.Name;

            this.Icon = ob.Icon;

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

        public OrderType Model()
        {
            return new OrderType
            {
                Name = this.Name,
                Icon = this.Icon,
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

        public string Name { get; set; }

        public string Icon { get; set; }
    }
}