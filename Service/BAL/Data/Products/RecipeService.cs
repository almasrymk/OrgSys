using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class RecipeService : BaseService<ProductRecipeModelView>
    {
        string Includes = "";
        UnitOfWork repo;
        public RecipeService()
        {
            repo = new UnitOfWork();
        }

        #region Save / Delete
        public ProductRecipeModelView Save(ProductRecipeModelView ob)
        {
            return new ProductRecipeModelView(repo.recipeRepo.AddOrUpdate(ob.Model()));
        }

        public bool Delete(long id)
        {
            return repo.recipeRepo.Delete(id);
        }

        public bool Delete(List<long> ids)
        {
            return repo.productUnitRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public ProductRecipeModelView Get(long Id)
        {
            return new ProductRecipeModelView(repo.recipeRepo.Get(e => e.Id == Id , Includes));
        }

        public ProductRecipeModelView Get(string textSearch)
        {
            return new ProductRecipeModelView(repo.recipeRepo.Get(null , Includes));
        }

        public List<ProductRecipeModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.recipeRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new ProductRecipeModelView(e)).ToList();
        }
         
        public List<ProductRecipeModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0)
        {
            return repo.recipeRepo.GetList(null, e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new ProductRecipeModelView(e)).ToList();
        }
         
        public IPagedList<ProductRecipeModelView> GetAll(long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.recipeRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new ProductRecipeModelView(e)).ToPagedList(page, pageSize);
        }
         
        public IPagedList<ProductRecipeModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.recipeRepo.GetList(null, e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new ProductRecipeModelView(e)).ToPagedList(page, pageSize);
        }
                
        public List<ProductRecipeModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.recipeRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new ProductRecipeModelView(e)).ToList();
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.recipeRepo.GetMaXCode();
        }
        #endregion
    }
}