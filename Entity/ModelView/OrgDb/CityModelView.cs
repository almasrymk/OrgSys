using Entity.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.ModelView
{
    public class CityModelView : BaseModel
    {
        //[StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        public long? CountryId { get; set; }

        public string CountryName { get; set; }
    }
}