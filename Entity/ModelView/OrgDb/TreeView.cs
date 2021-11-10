using Entity.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.ModelView
{
    public class TreeView : BaseModel
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public bool Select { get; set; }
    }
}