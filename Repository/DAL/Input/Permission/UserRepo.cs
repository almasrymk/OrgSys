using Entity.Model;

namespace Repository
{
    public class UserRepo : CurdOrg<User>
    {
        public UserRepo(string Schema) : base(Schema) { }
    }
}