using Entity.Model;

namespace Entity.ModelView
{
    public class JournalItemModelView : JournalItem
    {
        public string JournalCode { get; set; }

        public string AccountName { get; set; }
    }
}