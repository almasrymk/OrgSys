using AutoMapper;
using Entity.Model;
using Entity.ModelView;
using Application.Commands.Org.City;

public partial class MappingProfile : Profile
{
    public void CountryMappingProfile()
    {
        #region City
        CreateMap<Country, CountryModelView>();
        CreateMap<CountryModelView, Country>();
        //CreateMap<City, CreateCommand>();
        //CreateMap<CreateCommand, City>();
        //CreateMap<City, UpdateCommand>();
        //CreateMap<UpdateCommand, City>();
        //CreateMap<City, DeleteCommand>();
        //CreateMap<DeleteCommand, City>();
        #endregion
    }
}