using Utility;
using Entity.Model;

namespace Entity.ModelView
{
    public class PreferenceModelView : BaseModel
    {
        public PreferenceModelView()
        {

        }

        public PreferenceModelView(Preference ob)
        {
            if (ob == null)
                ob = new Preference();

            this.Key = ob.Key;

            this.Value = ob.Value;

            this.Reference = ob.Reference;

            this.UserId = ob.UserId;

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

        public Preference Model()
        {
            return new Preference
            {
                Key = this.Key,
                Value = this.Value,
                Reference = this.Reference,
                UserId = this.UserId,
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

        public string Key { get; set; }

        public string Value { get; set; }

        public string Reference { get; set; }

        public long? UserId { get; set; }
    }
}