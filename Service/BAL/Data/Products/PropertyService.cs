using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class PropertyService : BaseOrgService<PropertyModelView, Property>
    {
        public PropertyService(string Schema) : base(Schema, "PropertyElements") { }
    }
}