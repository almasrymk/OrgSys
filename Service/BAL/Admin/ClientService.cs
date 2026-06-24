using Domain.Entities;
using Application.DTOs;

namespace Service
{
    public class ClientService : BaseAdminService<ClientDto, Client>
    {
        public ClientService() : base("TypeActivity,Nationality") { }
       
        #region Gets      
        public ClientDto GetRequstId(long RequestId)
        {
            var ob = repo.Get(e => e.RequestId == RequestId, Includes).Map<ClientDto>();
            if (ob == null)
                ob = new ClientDto();
            return ob;
        }        

        public ClientDto GetClientName(string textSearch)
        {
            return repo.Get(e => e.Name.ToLower().Trim().Equals("" + textSearch.ToLower().Trim()), Includes).Map<ClientDto>();            
        }
       
        public bool CheckDoublicat(string ClientName, long id)
        {
            var ob = repo.Get(e => e.Name.Equals("" + ClientName) && (id == 0 || e.Id != id)).Map<ClientDto>();
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