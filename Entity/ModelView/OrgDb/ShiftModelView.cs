using System;
using Utility;
using Entity.Model;

namespace Entity.ModelView
{
    public class ShiftModelView : BaseModel
    {
        public ShiftModelView()
        {

        }

        public ShiftModelView(Shift ob)
        {
            if (ob == null)
                ob = new Shift();

            this.Name = ob.Name;

            this.Start = ob.Start;

            this.End = ob.End;

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

        public Shift Model()
        {
            return new Shift
            {
                Name = this.Name,
                Start = this.Start,
                End = this.End,
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

        public TimeSpan Start { get; set; }

        public TimeSpan End { get; set; }
    }
}