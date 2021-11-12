using Entity.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.ModelView
{
    public class ClientModelView : BaseModel
    {
        public ClientModelView()
        {

        }

        public ClientModelView(Client ob)
        {
            if (ob == null)
                ob = new Client();

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
            this.CompanyName = ob.CompanyName;
            this.Description = ob.Description;
            this.Phone = ob.Phone;
            this.Mobile = ob.Mobile;
            this.Fax = ob.Fax;
            this.Email = ob.Email;
            this.DbSchema = ob.DbSchema;
            this.TypeActivityId = ob.TypeActivityId;
            this.TypeActivityName = ob.TypeActivity?.Name;
            this.NationalityId = ob.NationalityId;
            this.NationalityName = ob.Nationality?.Name;
            this.SizeOfCompany = ob.SizeOfCompany;
            this.RequestId = ob.RequestId;
        }

        public Client Model()
        {
            return new Client
            {
                Id = this.Id,
                CodeNumber = this.CodeNumber,
                Code = this.Code,
                Name = this.Name,
                CompanyName = this.CompanyName,
                Description = this.Description,
                MaskText = this.MaskText,
                ParentId = this.ParentId,
                TypeId = this.TypeId,
                Hide = this.Hide,
                ImgPath = this.ImgPath,
                Status = this.Status,
                Phone = this.Phone,
                Mobile = this.Mobile,
                Fax = this.Fax,
                Email = this.Email,
                DbSchema = this.DbSchema,
                TypeActivityId = this.TypeActivityId,
                NationalityId = this.NationalityId,
                SizeOfCompany = this.SizeOfCompany,
                RequestId = this.RequestId
            };
        }

        [Required]
        public string Name { get; set; }

        public string CompanyName { get; set; }

        public string Description { get; set; }

        [StringLength(25, MinimumLength = 8)]
        public string Phone { get; set; }

        [StringLength(25, MinimumLength = 8)]
        public string Mobile { get; set; }

        [StringLength(25, MinimumLength = 8)]
        public string Fax { get; set; }

        [StringLength(30, MinimumLength = 3)]
        public string Email { get; set; }

        public string DbSchema { get; set; }

        public long TypeActivityId { get; set; }

        public string TypeActivityName { get; set; }

        public long NationalityId { get; set; }

        public string NationalityName { get; set; }

        public long SizeOfCompany { get; set; }

        public long RequestId { get; set; }
    }
}