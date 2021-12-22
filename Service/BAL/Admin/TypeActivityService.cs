using Entity;
using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class TypeActivityService : BaseAdminService<TypeActivityModelView, TypeActivity>
    {

        public TypeActivityService() { }

        #region Gets       
        public TypeActivityModelView GetTypeActivityName(string textSearch)
        {
            return repo.Get(e => e.Name.ToLower().Trim().Equals("" + textSearch.ToLower().Trim()), Includes).Map<TypeActivityModelView>();
        }

        public bool CheckDoublicat(string TypeActivityName, long id)
        {
            var ob = repo.Get(e => e.Name.Equals("" + TypeActivityName) && (id == 0 || e.Id != id)).Map<TypeActivityModelView>();
            return ob != null && ob.Id > 0;
        }
        #endregion
    }
}