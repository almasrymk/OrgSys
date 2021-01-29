using Entity.Model;
using Entity.ModelView;
using X.PagedList;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.BAL
{
    public class UserService : BaseService<UserModelView>
    {
        UnitOfWork repo;
        public UserService()
        {
            repo = new UnitOfWork();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public UserModelView Save(UserModelView ob)
        {
            return new UserModelView(repo.userRepo.AddOrUpdate(ob.Model));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(long id)
        {
            return repo.userRepo.Delete(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<UserModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.userRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new UserModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<UserModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.userRepo.GetList(e => e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new UserModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<UserModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.userRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new UserModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<UserModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.userRepo.GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new UserModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserModelView Get(long Id)
        {
            var ob =  new UserModelView(repo.userRepo.Get(e => e.Id == Id));
            if (ob == null)
                ob = new UserModelView();
            ob.Permissions = repo.permissionRepo.GetList(e=>e.OrderBy(e=>e.Id) , "").Select(e => new TreeView { Id = e.Id , Key = e.Key , Value = e.Value , ParentId = e.ParentId }).ToList();
            return ob;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public UserModelView Get(string textSearch)
        {
            var ob = new UserModelView(repo.userRepo.Get(e => e.Name.Equals("" + textSearch)));
            if (ob == null)
                ob = new UserModelView();
            ob.Permissions = repo.permissionRepo.GetList(e => e.OrderBy(e => e.Id), "").Select(e => new TreeView { Id = e.Id, Key = e.Key, Value = e.Value, ParentId = e.ParentId }).ToList();
            return ob;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public bool CheckDoublicat(string userName , long id)
        {
            var ob = new UserModelView(repo.userRepo.Get(e => e.UserName.Equals("" + userName) && (id == 0 || e.Id != id))) ;
            return ob != null && ob.Id > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(List<long> ids)
        {
            return repo.userRepo.Delete(ids);
        }

        public List<UserModelView> GetAll(List<long> ids)
        {
            return repo.userRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new UserModelView(e)).ToList();
        }
    }
}