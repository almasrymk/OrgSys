using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class ClassificationService : BaseOrgService<ClassificationModelView, Classification>
    {
        public ClassificationService(string Schema) : base(Schema) { }
    }
}