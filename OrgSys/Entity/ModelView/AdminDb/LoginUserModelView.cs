using Entity.Model;

namespace Entity.ModelView
{
    public class LoginUserModelView : LoginUser
    {
        public string ClientName { get; set; }

        public string Schema { get; set; }

        public string NewPassword { get; set; }

        public bool KeepLoggedIn { get; set; }
    }
}