using System.Collections.Generic;

namespace Entity.ModelView
{
    public class AccountTreeNodeModelView
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public List<AccountTreeNodeModelView> Children { get; set; } = new();
    }
}

