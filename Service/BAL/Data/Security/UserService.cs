using Domain.Entities;
using Application.DTOs;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using X.PagedList;
using X.PagedList.Extensions;

namespace Service
{
    public class UserService : BaseOrgService<UserDto, User>
    {
        public UnitOfWorkAdmin repoAdminAll;
        public UserService(string Schema) : base(Schema , "Role") {
            repoAdminAll = new UnitOfWorkAdmin();
        }

        public override Expression<Func<User, bool>> CreateFilter(string textSearch, long ParentId = 0, long TypeId = 0)
        {
            return e => "" + textSearch == "" || e.Name.ToLower().Contains(textSearch.ToLower()) || e.Code.ToLower().Contains(textSearch.ToLower());
        }

        //string Includes = "Role";
        //UnitOfWorkOrg repo;

        //public UserService(string Schema)
        //{
        //    repo = new UnitOfWorkOrg(Schema);
        //}

        #region Save / Delete
        public override UserDto Save(UserDto ob)
        {
            var comp = repoAll.companyProfileRepo.GetMyCompanyProfile();
            var usLogin = repoAdminAll.loginUserRepo.Get(e => e.Id == ob.LoginUserId);
            if (usLogin == null)
                usLogin = new LoginUser();
            usLogin.UserName = ob.UserName;
            usLogin.ClientId = comp.ClientId;           
            usLogin = repoAdminAll.loginUserRepo.AddOrUpdate(usLogin);
            ob.LoginUserId = usLogin.Id;
            var us = repo.AddOrUpdate(ob.Map<User>()).Map<UserDto>();
            ob.Id = us.Id;
            return ob;
        }

        public override bool Delete(long id)
        {
            return repo.Delete(id);
        }

        public override bool Delete(List<long> ids)
        {
            return repo.Delete(ids);
        }
        #endregion

        #region Gets
        public override UserDto Get(long Id)
        {
            var ob = repo.Get(e => e.Id == Id, Includes).Map<UserDto    >();
            if (ob == null)
                ob = new UserDto();

            var ids = repoAll.rolePermissionRepo.GetList(e => e.RoleId == ob.RoleId, null, "", Utility.Status.New).Select(e => e.PermissionId).Distinct().ToList();
            if (ids == null)
                ids = new List<long>();

            ob.Permissions = repoAll.permissionRepo.GetList(e => ids.Contains(e.Id), null, "", Utility.Status.New).ToList();
            return ob;
        }

        public UserDto GetByLoginUserId(long LoginUserId)
        {
            var ob = repo.Get(e => e.LoginUserId == LoginUserId, Includes).Map<UserDto>();
            if (ob == null)
                ob = new UserDto();

            var ids = repoAll.rolePermissionRepo.GetList(e => e.RoleId == ob.RoleId, null, "", Utility.Status.New).Select(e => e.PermissionId).Distinct().ToList();
            if (ids == null)
                ids = new List<long>();

            ob.Permissions = repoAll.permissionRepo.GetList(e => ids.Contains(e.Id), null, "", Utility.Status.New).ToList();
            return ob;
        }

        public override UserDto Get(string textSearch)
        {
            var ob = repo.Get(e => e.Name.ToLower().Trim().Equals("" + textSearch.ToLower().Trim()) || e.UserName.ToLower().Trim().Equals("" + textSearch.ToLower().Trim()), Includes).Map<UserDto>();
            if (ob == null)
                ob = new UserDto();

            var ids = repoAll.rolePermissionRepo.GetList(e => e.RoleId == ob.RoleId, null, "", Utility.Status.New).Select(e => e.PermissionId).Distinct().ToList();
            if (ids == null)
                ids = new List<long>();

            ob.Permissions = repoAll.permissionRepo.GetList(e => ids.Contains(e.Id), null, "", Utility.Status.New).ToList();

            return ob;
        }

        public UserDto GetUserName(string textSearch)
        {
            var ob = repo.Get(e => e.UserName.ToLower().Trim().Equals("" + textSearch.ToLower().Trim()), Includes).Map<UserDto>();
            if (ob == null)
                ob = new UserDto();

            var ids = repoAll.rolePermissionRepo.GetList(e => e.RoleId == ob.RoleId, null, "", Utility.Status.New).Select(e => e.PermissionId).Distinct().ToList();
            if (ids == null)
                ids = new List<long>();

            ob.Permissions = repoAll.permissionRepo.GetList(e => ids.Contains(e.Id), null, "", Utility.Status.New).ToList();

            return ob;
        }

        public override List<UserDto> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => e.Map<UserDto>()).ToList();
        }

        public override List<UserDto> GetAll(string textSearch, long parentId = 0, long TypeId = 0)
        {
            return repo.GetList(e => e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => e.Map<UserDto>()).ToList();
        }

        public override IPagedList<UserDto> GetAll(long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => e.Map<UserDto>()).ToPagedList(page, pageSize);
        }

        public override IPagedList<UserDto> GetAll(string textSearch, long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => e.Map<UserDto>()).ToPagedList(page, pageSize);
        }

        public bool CheckDoublicat(string userName, long id)
        {
            var ob = repo.Get(e => e.UserName.Equals("" + userName) && (id == 0 || e.Id != id));
            return ob != null && ob.Id > 0;
        }

        public override List<UserDto> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => e.Map<UserDto>()).ToList();
        }

        public override long GetMaxCode(long type = 0)
        {
            return repo.GetMaXCode();
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
        #endregion
    }
}