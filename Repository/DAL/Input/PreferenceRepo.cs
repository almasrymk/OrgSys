using Entity.Model;

namespace Repository
{
    public class PreferenceRepo : CurdOrg<Preference>
    {
        public PreferenceRepo(string Schema) : base(Schema) { }
    }
}