using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class TableService : BaseService<TableModelView>
    {
        string Includes = "";
        UnitOfWork repo;
        public TableService()
        {
            if (repo == null)
                repo = new UnitOfWork();
        }

        #region Save / Delete
        public TableModelView Save(TableModelView ob)
        {
            return new TableModelView(repo.tableRepo.AddOrUpdate(ob.Model));
        }

        public bool Delete(long id)
        {
            return repo.tableRepo.Delete(id);
        }

        public bool Delete(List<long> ids)
        {
            return repo.tableRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public TableModelView Get(long Id)
        {
            return new TableModelView(repo.tableRepo.Get(e => e.Id == Id , Includes));
        }

        public TableModelView Get(string textSearch)
        {
            return new TableModelView(repo.tableRepo.Get(e => e.Name.Contains("" + textSearch) , Includes));
        }

        public List<TableModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.tableRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new TableModelView(e)).ToList();
        }
      
        public List<TableModelView> GetAllClosed(long parentId = 0, long TypeId = 0)
        {
            var ids = repo.orderRepo.GetList(e => e.CloseTable != true, null, "", Utility.Status.New).Select(e=>e.TableId).Distinct().ToList();

            return repo.tableRepo.GetList(e => !ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new TableModelView(e)).ToList();
        }
      
        public List<TableModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.tableRepo.GetList(e => e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new TableModelView(e)).ToList();
        }
      
        public IPagedList<TableModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.tableRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new TableModelView(e)).ToPagedList(page, pageSize);
        }
       
        public IPagedList<TableModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.tableRepo.GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new TableModelView(e)).ToPagedList(page, pageSize);
        }           
       
        public List<TableModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.tableRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new TableModelView(e)).ToList();
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.tableRepo.GetMaXCode();
        }
        #endregion
    }
}