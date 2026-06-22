using Domain.Entities;
using Application.DTOs;

namespace Service
{
    public class ClientService : BaseAdminService<ClientModelView, Client>
    {
        public ClientService() : base("TypeActivity,Nationality") { }
       
        #region Gets      
        public ClientModelView GetRequstId(long RequestId)
        {
            var ob = repo.Get(e => e.RequestId == RequestId, Includes).Map<ClientModelView>();
            if (ob == null)
                ob = new ClientModelView();
            return ob;
        }        

        public ClientModelView GetClientName(string textSearch)
        {
            return repo.Get(e => e.Name.ToLower().Trim().Equals("" + textSearch.ToLower().Trim()), Includes).Map<ClientModelView>();            
        }
       
        public bool CheckDoublicat(string ClientName, long id)
        {
            var ob = repo.Get(e => e.Name.Equals("" + ClientName) && (id == 0 || e.Id != id)).Map<ClientModelView>();
            return ob != null && ob.Id > 0;
        }

        public bool CheckEmailToClient(string Email)
        {
            return repo.Any(e => e.Email.ToLower().Trim() == Email.ToLower().Trim());
        }

        public bool CheckPhoneToClient(string Mobile)
        {
            return repo.Any(e => e.Mobile.Trim() == Mobile.Trim());
        }
        #endregion
    }
}