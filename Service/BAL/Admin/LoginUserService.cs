using Domain.Entities;
using Application.DTOs;

namespace Service
{
    public class LoginUserService : BaseAdminService<LoginUserDto, LoginUser>
    {
        public LoginUserService() : base("Client") { }

        #region Gets 
        public LoginUserDto GetLoginUserName(string textSearch)
        {
            var ob = repo.Get(e => e.UserName.ToLower().Trim().Equals("" + textSearch.ToLower().Trim()), Includes).Map<LoginUserDto>();
            if (ob == null)
                ob = new LoginUserDto();
            return ob;
        }

        public bool CheckDoublicat(string LoginUserName, long id)
        {
            var ob = repo.Get(e => e.UserName.Equals("" + LoginUserName) && (id == 0 || e.Id != id)).Map<LoginUserDto>();
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

        public LoginUserDto GetUserByUserName(string Email)
        {
            return  repo.Get(e => e.UserName.ToLower().Trim() == Email.ToLower().Trim()).Map<LoginUserDto>();
        }
        #endregion
    }
}