using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class UserService : BaseOrgService<UserModelView, User>
    {
        public UserService(string Schema) : base(Schema) { }
    }
}