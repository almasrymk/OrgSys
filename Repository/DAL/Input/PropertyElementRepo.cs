using Domain.Entities;

namespace Repository
{
    public class PropertyElementRepo : CurdOrg<PropertyElement>
    {
        public PropertyElementRepo(string Schema) : base(Schema) { }
    }
}