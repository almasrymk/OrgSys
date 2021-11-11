using Entity.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace Entity.ModelView
{
    public class RequestModelView : BaseModel
    {
        public RequestModelView()
        {

        }

        public RequestModelView(Request ob)
        {
            if (ob == null)
                ob = new Request();

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
            this.Email = ob.Email;
            this.Phone = ob.Phone;
            this.CompanyName = ob.CompanyName;
            this.URL = ob.URL;
            this.Key = ob.Key;
            this.ExpireDate = ob.ExpireDate;

        }

        public Request Model()
        {
            return new Request
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
                Email = this.Email,
                Phone = this.Phone,
                CompanyName = this.CompanyName,
                URL = this.URL,
                Key = this.Key,
                ExpireDate = this.ExpireDate
            };
        }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string CompanyName { get; set; }
        public string URL { get; set; }
        public string Key { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}