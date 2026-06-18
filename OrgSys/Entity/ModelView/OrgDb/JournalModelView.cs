using Entity.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.ModelView
{
    public class JournalModelView : MovementModelView
    {
        public virtual long CurrencyId { get; set; }

        public virtual string CurrencyName { get; set; }

        public virtual long RefranceId { get; set; }

        public virtual long RefranceTypeId { get; set; }

        public virtual string RefranceTable { get; set; }

        public virtual string Note { get; set; }
        public ICollection<JournalItemModelView> JournalItems { get; set; }
    }
}