using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;
using Entity.Model;
using Microsoft.Extensions.Localization;

namespace Service
{
    public class UserService : BaseService<UserModelView>
    {
        string Includes = "Role";
        UnitOfWork repo;
        public UserService()
        {
            repo = new UnitOfWork();
        }
      
        #region Save / Delete
        public UserModelView Save(UserModelView ob)
        {
            return new UserModelView(repo.userRepo.AddOrUpdate(ob.Model()));
        }

        public bool Delete(long id)
        {
            return repo.userRepo.Delete(id);
        }

        public bool Delete(List<long> ids)
        {
            return repo.userRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public UserModelView Get(long Id)
        {
            var ob = new UserModelView(repo.userRepo.Get(e => e.Id == Id, Includes));
            if (ob == null)
                ob = new UserModelView();

            var ids = repo.rolePermissionRepo.GetList(e => e.RoleId == ob.RoleId, null, "", Utility.Status.New).Select(e => e.PermissionId).Distinct().ToList();
            if (ids == null)
                ids = new List<long>();

            ob.Permissions = repo.permissionRepo.GetList(e => ids.Contains(e.Id), null, "", Utility.Status.New).ToList();
            return ob;
        }

        public UserModelView Get(string textSearch)
        {
            var ob = new UserModelView(repo.userRepo.Get(e => e.Name.Equals("" + textSearch.ToLower()), Includes));
            if (ob == null)
                ob = new UserModelView();

            var ids = repo.rolePermissionRepo.GetList(e => e.RoleId == ob.RoleId, null, "", Utility.Status.New).Select(e => e.PermissionId).Distinct().ToList();
            if (ids == null)
                ids = new List<long>();

            ob.Permissions = repo.permissionRepo.GetList(e => ids.Contains(e.Id), null, "", Utility.Status.New).ToList();

            return ob;
        }

        public List<UserModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.userRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new UserModelView(e)).ToList();
        }

        public List<UserModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0)
        {
            return repo.userRepo.GetList(e => e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new UserModelView(e)).ToList();
        }

        public IPagedList<UserModelView> GetAll(long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.userRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new UserModelView(e)).ToPagedList(page, pageSize);
        }

        public IPagedList<UserModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.userRepo.GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new UserModelView(e)).ToPagedList(page, pageSize);
        }

        public bool CheckDoublicat(string userName, long id)
        {
            var ob = new UserModelView(repo.userRepo.Get(e => e.UserName.Equals("" + userName) && (id == 0 || e.Id != id)));
            return ob != null && ob.Id > 0;
        }

        public List<UserModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.userRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new UserModelView(e)).ToList();
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.userRepo.GetMaXCode();
        }

        public bool CheckEmail(string Email) => repo.userRepo.Any(e => e.UserName == Email);

        public bool HavePassword(string Email) => "" + repo.userRepo.Get(e => e.UserName == Email).Password != "";

        public bool CheckEmailAndPassword(string Email,string Passord) => repo.userRepo.Any(e => e.UserName== Email &&e.Password==Passord);

        #endregion
    }
}