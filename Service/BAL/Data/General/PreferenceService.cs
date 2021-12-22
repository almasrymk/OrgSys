using Entity;
using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class PreferenceService : BaseOrgService<PreferenceModelView, Preference>
    {
        public PreferenceService(string Schema) : base(Schema) { }

        public PreferenceModelView GetByKey(string textSearch, string reference, long type, int userId)
        {
            return repo.Get(e => e.Key == "" + textSearch && (e.TypeId == type || type == 0) && (userId == 0 || e.UserId == userId) && (e.Reference == reference || "" + reference == "")).Map<PreferenceModelView>();
        }
    }
}