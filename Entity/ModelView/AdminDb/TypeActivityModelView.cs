using Entity.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.ModelView
{
    public class TypeActivityModelView : BaseModel
    {
        public TypeActivityModelView()
        {

        }

        public TypeActivityModelView(TypeActivity ob)
        {
            if (ob == null)
                ob = new TypeActivity();

            this.Id = ob.Id;
            this.CodeNumber = ob.CodeNumber;
            this.Code = ob.Code;
            this.MaskText = ob.MaskText;
            this.ParentId = ob.ParentId;
            this.TypeId = ob.TypeId;
            this.Hide = ob.Hide;
            this.ImgPath = ob.ImgPath;
            this.Status = ob.Status;
            this.Name = ob.Name;           
        }

        public TypeActivity Model()
        {
            return new TypeActivity
            {
                Id = this.Id,
                CodeNumber = this.CodeNumber,
                Code = this.Code,
                MaskText = this.MaskText,
                ParentId = this.ParentId,
                TypeId = this.TypeId,
                Hide = this.Hide,
                ImgPath = this.ImgPath,
                Status = this.Status,
                Name = this.Name,               
            };
        }

        [Required]
        public string Name { get; set; }       
    }
}