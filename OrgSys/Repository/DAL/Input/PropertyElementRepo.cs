using Entity.Model;

namespace Repository
{
    public class PropertyElementRepo : CurdOrg<PropertyElement>
    {
        public PropertyElementRepo(string Schema) : base(Schema) { }
    }
}