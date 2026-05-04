using Entity.Model;
using Entity.ModelView;
namespace Service
{
    public class PropertyElementService : BaseOrgService<PropertyElementModelView, PropertyElement>
    {
        public PropertyElementService(string Schema) : base(Schema , "Property") { }
    }
}