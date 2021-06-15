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
    public class TableService : BaseService<TableModelView>
    {
        UnitOfWork repo;
        public TableService()
        {
            if (repo == null)
                repo = new UnitOfWork();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public TableModelView Save(TableModelView ob)
        {
            return new TableModelView(repo.tableRepo.AddOrUpdate(ob.Model));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(long id)
        {
            return repo.tableRepo.Delete(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TableModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.tableRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new TableModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TableModelView> GetAllClosed(long parentId = 0, long TypeId = 0)
        {
            var ids = repo.orderRepo.GetList(e => e.CloseTable != true, null, "", Utility.Status.New).Select(e=>e.TableId).Distinct().ToList();

            return repo.tableRepo.GetList(e => !ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new TableModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<TableModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.tableRepo.GetList(e => e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new TableModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<TableModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.tableRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new TableModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<TableModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.tableRepo.GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new TableModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TableModelView Get(long Id)
        {
            return new TableModelView(repo.tableRepo.Get(e => e.Id == Id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public TableModelView Get(string textSearch)
        {
            return new TableModelView(repo.tableRepo.Get(e => e.Name.Contains("" + textSearch)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(List<long> ids)
        {
            return repo.tableRepo.Delete(ids);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="TypeId"></param>
        /// <returns></returns>
        public List<TableModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.tableRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new TableModelView(e)).ToList();
        }
    }
}