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
    public class PreferenceService : BaseService<PreferenceModelView>
    {
        UnitOfWork repo;
        public PreferenceService()
        {
            repo = new UnitOfWork();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public PreferenceModelView Save(PreferenceModelView ob)
        {
            return new PreferenceModelView(repo.preferenceRepo.AddOrUpdate(ob.Model));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(long id)
        {
            return repo.preferenceRepo.Delete(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<PreferenceModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.preferenceRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new PreferenceModelView(e)).ToList();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<PreferenceModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.preferenceRepo.GetList(e=> (e.Reference == "" + textSearch && e.TypeId == TypeId), e => e.OrderBy(e => e.Id), "", Utility.Status.All).Select(e => new PreferenceModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<PreferenceModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.preferenceRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new PreferenceModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<PreferenceModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.preferenceRepo.GetList(e => "" + textSearch == "" || e.Key.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new PreferenceModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PreferenceModelView Get(long Id)
        {
            return new PreferenceModelView(repo.preferenceRepo.Get(e => e.Id == Id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public PreferenceModelView Get(string textSearch)
        {
            return new PreferenceModelView(repo.preferenceRepo.Get(e => e.Key.Contains("" + textSearch)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(List<long> ids)
        {
            return repo.preferenceRepo.Delete(ids);
        }

        public List<PreferenceModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.preferenceRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new PreferenceModelView(e)).ToList();
        }

        public PreferenceModelView GetByKey(string textSearch , string reference , long type , int userId)
        {
            return new PreferenceModelView(repo.preferenceRepo.Get(e => e.Key =="" + textSearch && (e.TypeId == type || type == 0) && (userId == 0 || e.UserId == userId) && (e.Reference == reference || "" + reference == "") ));
        }
    }
}