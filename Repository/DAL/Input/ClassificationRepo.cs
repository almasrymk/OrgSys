using Domain.Entities;

namespace Repository
{
    public class ClassificationRepo : CurdOrg<Classification>
    {
        public ClassificationRepo(string Schema) : base(Schema) { }
    }
}