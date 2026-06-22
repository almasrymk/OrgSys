using Domain.Entities;

namespace Repository
{
    public class PropertyRepo : CurdOrg<Property>
    {
        public PropertyRepo(string Schema) : base(Schema) { }
    }
}