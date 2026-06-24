using Entity;
using Domain.Entities;
using Application.DTOs;

namespace Service
{
    public class NationalityService : BaseAdminService<NationalityModelView, Nationality>
    {
        public NationalityService() { }
        
        #region Gets       
        public NationalityModelView GetNationalityName(string textSearch)
        {
           return repo.Get(e => e.Name.ToLower().Trim().Equals("" + textSearch.ToLower().Trim()), Includes).Map<NationalityModelView>();          
        }

        public bool CheckDoublicat(string NationalityName, long id)
        {
            var ob = repo.Get(e => e.Name.Equals("" + NationalityName) && (id == 0 || e.Id != id)).Map<NationalityModelView>();
            return ob != null && ob.Id > 0;
        }    
        #endregion
    }
}