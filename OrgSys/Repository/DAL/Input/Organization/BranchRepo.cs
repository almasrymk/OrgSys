using Entity.Model;

namespace Repository
{
    public class BranchRepo : CurdOrg<Branch>
    {
        public BranchRepo(string Schema) : base(Schema) { }
    }
}