using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;
using Entity.Model;
using Microsoft.Extensions.Localization;

namespace Service
{
    public class LoginUserService : BaseService<LoginUserModelView>
    {
        string Includes = "Client";
        UnitOfWorkAdmin repo;
        
        public LoginUserService()
        {
            repo = new UnitOfWorkAdmin();
        }

        #region Save / Delete
        public LoginUserModelView Save(LoginUserModelView ob)
        {
            return new LoginUserModelView(repo.loginUserRepo.AddOrUpdate(ob.Model()));
        }

        public bool Delete(long id)
        {
            return repo.loginUserRepo.Delete(id);
        }

        public bool Delete(List<long> ids)
        {
            return repo.loginUserRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public LoginUserModelView Get(long Id)
        {
            var ob = new LoginUserModelView(repo.loginUserRepo.Get(e => e.Id == Id, Includes));
            if (ob == null)
                ob = new LoginUserModelView();         
            return ob;
        }

        public LoginUserModelView Get(string textSearch)
        {
            var ob = new LoginUserModelView(repo.loginUserRepo.Get(e => e.UserName.ToLower().Trim().Equals("" + textSearch.ToLower().Trim()), Includes));
            if (ob == null)
                ob = new LoginUserModelView();           
            return ob;
        }

        public LoginUserModelView GetLoginUserName(string textSearch)
        {
            var ob = new LoginUserModelView(repo.loginUserRepo.Get(e => e.UserName.ToLower().Trim().Equals("" + textSearch.ToLower().Trim()), Includes));
            if (ob == null)
                ob = new LoginUserModelView();           
            return ob;
        }

        public List<LoginUserModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.loginUserRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new LoginUserModelView(e)).ToList();
        }

        public List<LoginUserModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0)
        {
            return repo.loginUserRepo.GetList(e => e.UserName.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new LoginUserModelView(e)).ToList();
        }

        public IPagedList<LoginUserModelView> GetAll(long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.loginUserRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new LoginUserModelView(e)).ToPagedList(page, pageSize);
        }

        public IPagedList<LoginUserModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.loginUserRepo.GetList(e => "" + textSearch == "" || e.UserName.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new LoginUserModelView(e)).ToPagedList(page, pageSize);
        }

        public bool CheckDoublicat(string LoginUserName, long id)
        {
            var ob = new LoginUserModelView(repo.loginUserRepo.Get(e => e.UserName.Equals("" + LoginUserName) && (id == 0 || e.Id != id)));
            return ob != null && ob.Id > 0;
        }

        public List<LoginUserModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.loginUserRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new LoginUserModelView(e)).ToList();
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.loginUserRepo.GetMaXCode();
        }

        public bool CheckEmail(string Email) => repo.loginUserRepo.Any(e => e.UserName.ToLower().Trim() == Email.ToLower().Trim());

        public bool CheckCurrentPassword(long Id, string CurrentPassword) => repo.loginUserRepo.Any(e => e.Id == Id && e.Password == CurrentPassword);

        public bool HavePassword(string Email) => "" + repo.loginUserRepo.Get(e => e.UserName.ToLower().Trim() == Email.ToLower().Trim())?.Password != "";

        public bool CheckEmailAndPassword(string Email,string Passord) => repo.loginUserRepo.Any(e => e.UserName.ToLower().Trim() == Email.ToLower().Trim() && e.Password==Passord);
        #endregion
    }
}