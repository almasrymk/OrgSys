using Domain.Entities;
using Application.DTOs;

namespace Service.BAL.Data.Security
{
    public class RequestService : BaseAdminService<RequestDto, Request>
    {
        public RequestService() { }

        #region Gets       
        public RequestDto GetEmail(string Email)
        {
            return repo.Get(e => e.Email.Contains("" + Email), Includes).Map<RequestDto>();

        }
        public RequestDto GetByKey(string Key)
        {
            return repo.Get(e => e.Key == Key, Includes).Map<RequestDto>();
        }
        #endregion
    }
}