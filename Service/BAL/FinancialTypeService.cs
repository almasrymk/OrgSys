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
    public class FinancialTypeService : BaseService<FinancialTypeModelView>
    {
        UnitOfWork repo;
        public FinancialTypeService()
        {
            if (repo == null)
                repo = new UnitOfWork();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public FinancialTypeModelView Save(FinancialTypeModelView ob)
        {
            return new FinancialTypeModelView(repo.financialTypeRepo.AddOrUpdate(ob.Model));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(long id)
        {
            return repo.financialTypeRepo.Delete(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<FinancialTypeModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.financialTypeRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new FinancialTypeModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<FinancialTypeModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.financialTypeRepo.GetList(e => e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new FinancialTypeModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<FinancialTypeModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.financialTypeRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new FinancialTypeModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<FinancialTypeModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.financialTypeRepo.GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new FinancialTypeModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FinancialTypeModelView Get(long Id)
        {
            return new FinancialTypeModelView(repo.financialTypeRepo.Get(e => e.Id == Id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public FinancialTypeModelView Get(string textSearch)
        {
            return new FinancialTypeModelView(repo.financialTypeRepo.Get(e => e.Name.Contains("" + textSearch)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(List<long> ids)
        {
            return repo.financialTypeRepo.Delete(ids);
        }

        public List<FinancialTypeModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.financialTypeRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new FinancialTypeModelView(e)).ToList();
        }
    }
}