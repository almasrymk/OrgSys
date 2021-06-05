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
   public class RecipeService : BaseService<ProductRecipeModelView>
    {
        UnitOfWork repo;
        public RecipeService()
        {
            repo = new UnitOfWork();
        }
        public ProductRecipeModelView Save(ProductRecipeModelView ob)
        {
            return new ProductRecipeModelView(repo.recipeRepo.AddOrUpdate(ob.Model));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(long id)
        {
            return repo.recipeRepo.Delete(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ProductRecipeModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.recipeRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new ProductRecipeModelView(e)).ToList();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<ProductRecipeModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0)
        {
            return repo.recipeRepo.GetList(null, e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new ProductRecipeModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<ProductRecipeModelView> GetAll(long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.recipeRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new ProductRecipeModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<ProductRecipeModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.recipeRepo.GetList(null, e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new ProductRecipeModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProductRecipeModelView Get(long Id)
        {
            return new ProductRecipeModelView(repo.recipeRepo.Get(e => e.Id == Id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public ProductRecipeModelView Get(string textSearch)
        {
            return new ProductRecipeModelView(repo.recipeRepo.Get(null));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(List<long> ids)
        {
            return repo.productUnitRepo.Delete(ids);
        }

        public List<ProductRecipeModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.recipeRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new ProductRecipeModelView(e)).ToList();
        }
    }
}
