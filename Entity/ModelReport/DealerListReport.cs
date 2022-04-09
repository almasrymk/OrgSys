namespace Entity.ModelReport
{
    public class DealerListReport : BaseModel
    {
        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public long? DealerGroupId { get; set; }

        public string DealerGroupName { get; set; }

        public long? CountryId { get; set; }

        public string CountryName { get; set; }

        public long? CityId { get; set; }

        public string CityName { get; set; }

        public long? DistrictId { get; set; }

        public string DistrictName { get; set; }
    }
}