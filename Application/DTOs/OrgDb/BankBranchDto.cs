using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class BankBranchDto : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        public long BankId { get; set; }

        public long CountryId { get; set; }

        public long CityId { get; set; }

        public long DistrictId { get; set; }

        public string BankName { get; set; }

        public string CountryName { get; set; }

        public string CityName { get; set; }

        public string DistrictName { get; set; }
    }
}