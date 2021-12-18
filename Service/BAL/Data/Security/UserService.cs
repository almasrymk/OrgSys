using Entity;
using Entity.Model;
using Entity.ModelView;
using System.Collections.Generic;
using System.Linq;
using X.PagedList;

namespace Service
{
    public class UserService : BaseOrgService<UserModelView, User>
    {
        public UserService(string Schema) : base(Schema) { }

        //string Includes = "Role";
        //UnitOfWorkOrg repo;

        //public UserService(string Schema)
        //{
        //    repo = new UnitOfWorkOrg(Schema);
        //}

        #region Save / Delete
        public override UserModelView Save(UserModelView ob)
        {
            return repo.GetRepo<UserModelView>().AddOrUpdate(ob.Map<UserModelView>());
        }

        public override bool Delete(long id)
        {
            return repo.GetRepo<UserModelView>().Delete(id);
        }

        public override bool Delete(List<long> ids)
        {
            return repo.GetRepo<User>().Delete(ids);
        }
        #endregion

        #region Gets
        public override UserModelView Get(long Id)
        {
            var ob = repo.GetRepo<UserModelView>().Get(e => e.Id == Id, Includes);
            if (ob == null)
                ob = new UserModelView();

            var ids = repo.GetRepo<RolePermission>().GetList(e => e.RoleId == ob.RoleId, null, "", Utility.Status.New).Select(e => e.PermissionId).Distinct().ToList();
            if (ids == null)
                ids = new List<long>();

            ob.Permissions = repo.GetRepo<Permission>().GetList(e => ids.Contains(e.Id), null, "", Utility.Status.New).ToList();
            return ob;
        }

        public UserModelView GetByLoginUserId(long LoginUserId)
        {
            var ob = repo.GetRepo<UserModelView>().Get(e => e.LoginUserId == LoginUserId, Includes);
            if (ob == null)
                ob = new UserModelView();

            var ids = repo.GetRepo<RolePermission>().GetList(e => e.RoleId == ob.RoleId, null, "", Utility.Status.New).Select(e => e.PermissionId).Distinct().ToList();
            if (ids == null)
                ids = new List<long>();

            ob.Permissions = repo.GetRepo<Permission>().GetList(e => ids.Contains(e.Id), null, "", Utility.Status.New).ToList();
            return ob;
        }

        public override UserModelView Get(string textSearch)
        {
            var ob = repo.GetRepo<UserModelView>().Get(e => e.Name.ToLower().Trim().Equals("" + textSearch.ToLower().Trim()) || e.UserName.ToLower().Trim().Equals("" + textSearch.ToLower().Trim()), Includes);
            if (ob == null)
                ob = new UserModelView();

            var ids = repo.GetRepo<RolePermission>().GetList(e => e.RoleId == ob.RoleId, null, "", Utility.Status.New).Select(e => e.PermissionId).Distinct().ToList();
            if (ids == null)
                ids = new List<long>();

            ob.Permissions = repo.GetRepo<Permission>().GetList(e => ids.Contains(e.Id), null, "", Utility.Status.New).ToList();

            return ob;
        }

        public UserModelView GetUserName(string textSearch)
        {
            var ob = repo.GetRepo<UserModelView>().Get(e => e.UserName.ToLower().Trim().Equals("" + textSearch.ToLower().Trim()), Includes);
            if (ob == null)
                ob = new UserModelView();

            var ids = repo.GetRepo<RolePermission>().GetList(e => e.RoleId == ob.RoleId, null, "", Utility.Status.New).Select(e => e.PermissionId).Distinct().ToList();
            if (ids == null)
                ids = new List<long>();

            ob.Permissions = repo.GetRepo<Permission>().GetList(e => ids.Contains(e.Id), null, "", Utility.Status.New).ToList();

            return ob;
        }


        public override List<UserModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.GetRepo<UserModelView>().GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e =>e.Map<UserModelView>()).ToList();
        }

        public override List<UserModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0)
        {
            return repo.GetRepo<UserModelView>().GetList(e => e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => e.Map<UserModelView>()).ToList();
        }

        public override IPagedList<UserModelView> GetAll(long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.GetRepo<UserModelView>().GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e =>e.Map<UserModelView>()).ToPagedList(page, pageSize);
        }

        public override IPagedList<UserModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.GetRepo<UserModelView>().GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e =>e.Map<UserModelView>()).ToPagedList(page, pageSize);
        }

        public bool CheckDoublicat(string userName, long id)
        {
            var ob = repo.GetRepo<UserModelView>().Get(e => e.UserName.Equals("" + userName) && (id == 0 || e.Id != id));
            return ob != null && ob.Id > 0;
        }

        public override List<UserModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.GetRepo<UserModelView>().GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => e.Map<UserModelView>()).ToList();
        }

        public override long GetMaxCode(long type = 0)
        {
            return repo.GetRepo<UserModelView>().GetMaXCode();
        }

        public bool CheckEmail(string Email) => repo.GetRepo<UserModelView>().Any(e => e.UserName.ToLower().Trim() == Email.ToLower().Trim());
        public bool CheckCurrentPassword(long Id, string CurrentPassword) => repo.GetRepo<UserModelView>().Any(e => e.Id == Id && e.Password == CurrentPassword);
        public bool HavePassword(string Email) => "" + repo.GetRepo<UserModelView>().Get(e => e.UserName.ToLower().Trim() == Email.ToLower().Trim())?.Password != "";

        public bool CheckEmailAndPassword(string Email, string Passord) => repo.GetRepo<UserModelView>().Any(e => e.UserName.ToLower().Trim() == Email.ToLower().Trim() && e.Password == Passord);

        #endregion
    }
}