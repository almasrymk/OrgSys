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
    public class DealerService : BaseService<DealerModelView>
    {
        UnitOfWork repo;
        public DealerService()
        {
            repo = new UnitOfWork();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public DealerModelView Save(DealerModelView ob)
        {
            return new DealerModelView(repo.dealerRepo.AddOrUpdate(ob.Model));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(long id)
        {
            return repo.dealerRepo.Delete(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<DealerModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.dealerRepo.GetList(e=>e.TypeId == TypeId , e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new DealerModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<DealerModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.dealerRepo.GetList(e => e.TypeId == TypeId && e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new DealerModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<DealerModelView> GetAll(long parentId = 0, long TypeId = 0 , int page = 1, int pageSize = 20)
        {
            return repo.dealerRepo.GetList(e=> e.TypeId == TypeId , e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new DealerModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<DealerModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.dealerRepo.GetList(e => e.TypeId == TypeId &&( "" + textSearch == "" || e.Name.Contains("" + textSearch)), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new DealerModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DealerModelView Get(long Id)
        {
            return new DealerModelView(repo.dealerRepo.Get(e => e.Id == Id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public DealerModelView Get(string textSearch)
        {
            return new DealerModelView(repo.dealerRepo.Get(e => e.Name.Contains("" + textSearch)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(List<long> ids)
        {
            return repo.dealerRepo.Delete(ids);
        }

        public List<DealerModelView> GetAll(List<long> ids)
        {
            return repo.dealerRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new DealerModelView(e)).ToList();
        }
    }
}