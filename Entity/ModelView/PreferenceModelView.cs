using Utility;
using Entity.Model;
using Utility.Resource;
using System.ComponentModel.DataAnnotations;

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
            this.Id = ob.Id;
            this.Key = ob.Key;
            this.Value = ob.Value;
            this.Reference = ob.Reference;
            this.UserId = ob.UserId;
            this.Status = ob.Status;
            this.ParentId = ob.ParentId;
            this.TypeId = ob.TypeId;
            this.ImgPath = ob.ImgPath;
        }

        public Preference Model
        {
            get
            {
                return new Preference
                {
                    Id = this.Id,
                    Key = this.Key,
                    Value = this.Value,
                    Reference = this.Reference,
                    UserId = this.UserId,
                    Status = this.Status,
                    MaskText = this.MaskText,
                    ParentId = this.ParentId,
                    TypeId = this.TypeId,
                    ImgPath = this.ImgPath
                };
            }
        }

        public string Key { get; set; }
        public string Value { get; set; }
        public string Reference { get; set; }
        public long? UserId { get; set; }
    }
}