using Domain.Entities;
using Application.DTOs;

namespace Service
{
    public class TypeActivityService : BaseAdminService<TypeActivityDto, TypeActivity>
    {

        public TypeActivityService() { }

        #region Gets       
        public TypeActivityDto GetTypeActivityName(string textSearch)
        {
            return repo.Get(e => e.Name.ToLower().Trim().Equals("" + textSearch.ToLower().Trim()), Includes).Map<TypeActivityDto>();
        }

        public bool CheckDoublicat(string TypeActivityName, long id)
        {
            var ob = repo.Get(e => e.Name.Equals("" + TypeActivityName) && (id == 0 || e.Id != id)).Map<TypeActivityDto>();
            return ob != null && ob.Id > 0;
        }
        #endregion
    }
}