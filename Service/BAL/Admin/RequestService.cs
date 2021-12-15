using Entity;
using Entity.Model;
using Entity.ModelView;

namespace Service.BAL.Data.Security
{
    public class RequestService : BaseAdminService<RequestModelView, Request>
    {
        public RequestService() { }

        #region Gets       
        public RequestModelView GetEmail(string Email)
        {
            return repo.Db.Get(e => e.Email.Contains("" + Email), Includes).Map<RequestModelView>();

        }
        public RequestModelView GetByKey(string Key)
        {
            return repo.Db.Get(e => e.Key == Key, Includes).Map<RequestModelView>();
        }
        #endregion
    }
}