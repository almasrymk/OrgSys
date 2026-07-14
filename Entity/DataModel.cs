using System;
using Entity.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    public class DataModel : BaseModel
    {
        public virtual string Name { get; set; }
    }
}