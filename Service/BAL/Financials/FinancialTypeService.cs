using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class FinancialTypeService : IBaseService<FinancialTypeModelView>
    {
        string Includes = "";
        UnitOfWorkOrg repo;
        private string _Schema;
        public FinancialTypeService(string Schema)
        {
            this._Schema = Schema;
            repo = new UnitOfWorkOrg(Schema);
        }

        #region Save / Delete
        public FinancialTypeModelView Save(FinancialTypeModelView ob)
        {
            return new FinancialTypeModelView(repo.financialTypeRepo.AddOrUpdate(ob.Model()));
        }
         
        public bool Delete(long id)
        {
            return repo.financialTypeRepo.Delete(id);
        }

        public bool Delete(List<long> ids)
        {
            return repo.financialTypeRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public FinancialTypeModelView Get(long Id)
        {
            return new FinancialTypeModelView(repo.financialTypeRepo.Get(e => e.Id == Id , Includes));
        }

        public FinancialTypeModelView Get(string textSearch)
        {
            return new FinancialTypeModelView(repo.financialTypeRepo.Get(e => e.Name.Contains("" + textSearch) , Includes));
        }

        public List<FinancialTypeModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.financialTypeRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new FinancialTypeModelView(e)).ToList();
        }
         
        public List<FinancialTypeModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.financialTypeRepo.GetList(e => e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new FinancialTypeModelView(e)).ToList();
        }
         
        public IPagedList<FinancialTypeModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.financialTypeRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new FinancialTypeModelView(e)).ToPagedList(page, pageSize);
        }
         
        public IPagedList<FinancialTypeModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.financialTypeRepo.GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new FinancialTypeModelView(e)).ToPagedList(page, pageSize);
        }
                
        public List<FinancialTypeModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.financialTypeRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new FinancialTypeModelView(e)).ToList();
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.financialTypeRepo.GetMaXCode();
        }
        #endregion
    }
}