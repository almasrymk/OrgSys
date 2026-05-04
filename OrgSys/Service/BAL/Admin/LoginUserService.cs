using Entity;
using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class LoginUserService : BaseAdminService<LoginUserModelView, LoginUser>
    {
        public LoginUserService() : base("Client") { }

        #region Gets 
        public LoginUserModelView GetLoginUserName(string textSearch)
        {
            var ob = repo.Get(e => e.UserName.ToLower().Trim().Equals("" + textSearch.ToLower().Trim()), Includes).Map<LoginUserModelView>();
            if (ob == null)
                ob = new LoginUserModelView();
            return ob;
        }

        public bool CheckDoublicat(string LoginUserName, long id)
        {
            var ob = repo.Get(e => e.UserName.Equals("" + LoginUserName) && (id == 0 || e.Id != id)).Map<LoginUserModelView>();
            return ob != null && ob.Id > 0;
        }

        public bool CheckEmail(string Email)
        {
            return repo.Any(e => e.UserName.ToLower().Trim() == Email.ToLower().Trim());
        }

        public bool CheckCurrentPassword(long Id, string CurrentPassword)
        {
            return repo.Any(e => e.Id == Id && e.Password == CurrentPassword);
        }

        public bool HavePassword(string Email)
        {
            return "" + repo.Get(e => e.UserName.ToLower().Trim() == Email.ToLower().Trim())?.Password != "";
        }

        public bool CheckEmailAndPassword(string Email, string Passord)
        {
            return repo.Any(e => e.UserName.ToLower().Trim() == Email.ToLower().Trim() && e.Password == Passord);
        }

        public LoginUserModelView GetUserByUserName(string Email)
        {
            return  repo.Get(e => e.UserName.ToLower().Trim() == Email.ToLower().Trim()).Map<LoginUserModelView>();
        }
        #endregion
    }
}