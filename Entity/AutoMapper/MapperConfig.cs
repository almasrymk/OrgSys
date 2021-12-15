using AutoMapper;
using Entity.Model;
using Entity.ModelView;
using AutoMapper.Configuration;

namespace Entity
{
    public class MapperConfig : Profile
    {
        public IMapper Mapper { get; set; }
        public MapperConfig()
        {
            var cfg = new MapperConfigurationExpression();

            #region Admin
            // Client
            cfg.CreateMap<ClientModelView, Client>();
            cfg.CreateMap<Client, ClientModelView>()
            .ForMember(d => d.TypeActivityName, o => o.MapFrom(s => s.TypeActivity.Name))
            .ForMember(d => d.NationalityName, o => o.MapFrom(s => s.Nationality.Name));
            //

            // Client Plan
            cfg.CreateMap<ClientPlanModelView, ClientPlan>();
            cfg.CreateMap<ClientPlan, ClientPlanModelView>()
            .ForMember(d => d.ClientName, o => o.MapFrom(s => s.Client.Name))
            .ForMember(d => d.PlanName, o => o.MapFrom(s => s.Plan.Name));
            //

            // General City
            cfg.CreateMap<GeneralCityModelView, GeneralCity>();
            cfg.CreateMap<GeneralCity, GeneralCityModelView>()
            .ForMember(d => d.GeneralCountryId, o => o.MapFrom(s => s.GeneralCountry.Name));
            //

            // General Classification
            cfg.CreateMap<GeneralClassificationModelView, GeneralClassification>();
            cfg.CreateMap<GeneralClassification, GeneralClassificationModelView>();
            //

            // General Country
            cfg.CreateMap<GeneralCountryModelView, GeneralCountry>();
            cfg.CreateMap<GeneralCountry, GeneralCountryModelView>();
            //

            // General District
            cfg.CreateMap<GeneralDistrictModelView, GeneralDistrict>();
            cfg.CreateMap<GeneralDistrict, GeneralDistrictModelView>()
            .ForMember(d => d.GeneralCountryName, o => o.MapFrom(s => s.GeneralCountry.Name))
            .ForMember(d => d.GeneralCityName, o => o.MapFrom(s => s.GeneralCity.Name));
            //

            // General Product
            cfg.CreateMap<GeneralProductModelView, GeneralProduct>();
            cfg.CreateMap<GeneralProduct, GeneralProductModelView>()
            .ForMember(d => d.GeneralClassificationName, o => o.MapFrom(s => s.GeneralClassification.Name))
            .ForMember(d => d.GeneralProductUnitList, o => o.MapFrom(s => s.GeneralProductUnits))
            .ForMember(d => d.GeneralProductRecipeList, o => o.MapFrom(s => s.GeneralProductRecipes))
            .ForMember(d => d.GeneralProductPropertyElementList, o => o.MapFrom(s => s.GeneralProductPropertyElements));
            //

            // General Product Property Element
            cfg.CreateMap<GeneralProductPropertyElementModelView, GeneralProductPropertyElement>();
            cfg.CreateMap<GeneralProductPropertyElement, GeneralProductPropertyElementModelView>()
            .ForMember(d => d.GeneralProductName, o => o.MapFrom(s => s.GeneralProduct.Name))
            .ForMember(d => d.GeneralPropertyName, o => o.MapFrom(s => s.GeneralProperty.Name))
            .ForMember(d => d.GeneralPropertyElementName, o => o.MapFrom(s => s.GeneralPropertyElement.Name));
            //


            // General Product Recipe
            cfg.CreateMap<GeneralProductRecipeModelView, GeneralProductRecipe>();
            cfg.CreateMap<GeneralProductRecipe, GeneralProductRecipeModelView>();
            //

            // General Product Unit
            cfg.CreateMap<GeneralProductUnitModelView, GeneralProductUnit>();
            cfg.CreateMap<GeneralProductUnit, GeneralProductUnitModelView>()
            .ForMember(d => d.GeneralProductName, o => o.MapFrom(s => s.GeneralProduct.Name))
            .ForMember(d => d.GeneralUnitName, o => o.MapFrom(s => s.GeneralUnit.Name));
            //

            // General Property Element
            cfg.CreateMap<GeneralPropertyElementModelView, GeneralPropertyElement>();
            cfg.CreateMap<GeneralPropertyElement, GeneralPropertyElementModelView>()
            .ForMember(d => d.GeneralPropertyName, o => o.MapFrom(s => s.GeneralProperty.Name));
            //

            // General Property
            cfg.CreateMap<GeneralPropertyModelView, GeneralProperty>();
            cfg.CreateMap<GeneralProperty, GeneralPropertyModelView>()
            .ForMember(d => d.GeneralPropertyElementList, o => o.MapFrom(s => s.GeneralPropertyElements));
            //

            // General Unit
            cfg.CreateMap<GeneralUnitModelView, GeneralUnit>();
            cfg.CreateMap<GeneralUnit, GeneralUnitModelView>();
            //

            // Login User
            cfg.CreateMap<LoginUserModelView, LoginUser>()
            .ForMember(d => d.Password, o => o.MapFrom(s => Utility.Security.Encrypt(s.Password)));
            cfg.CreateMap<LoginUser, LoginUserModelView>()
            .ForMember(d => d.ClientName, o => o.MapFrom(s => s.Client.Name))
            .ForMember(d => d.Schema, o => o.MapFrom(s => s.Client.DbSchema))
             .ForMember(d => d.Password, o => o.MapFrom(s => Utility.Security.Decrypt(s.Password)));
            //
            
            // Nationality
            cfg.CreateMap<NationalityModelView, Nationality>();
            cfg.CreateMap<Nationality, NationalityModelView>();
            //

            // Plan Element
            cfg.CreateMap<PlanElementModelView, PlanElement>();
            cfg.CreateMap<PlanElement, PlanElementModelView>()
            .ForMember(d => d.PlanName, o => o.MapFrom(s => s.Plan.Name));
            //

            // Plan
            cfg.CreateMap<PlanModelView, Plan>();
            cfg.CreateMap<Plan, PlanModelView>()
            .ForMember(d => d.PlanTypeName, o => o.MapFrom(s => s.PlanType.Name))
            .ForMember(d => d.PlanElementList, o => o.MapFrom(s => s.PlanElements));
            //

            // Plan Type
            cfg.CreateMap<PlanTypeModelView, PlanType>();
            cfg.CreateMap<PlanType, PlanTypeModelView>();
            //

            // Request
            cfg.CreateMap<RequestModelView, Request>();
            cfg.CreateMap<Request, RequestModelView>();
            //

            // Type Activity
            cfg.CreateMap<TypeActivityModelView, TypeActivity>();
            cfg.CreateMap<TypeActivity, TypeActivityModelView>();
            //
            #endregion

            cfg.AddProfile(this);
            Mapper = new MapperConfiguration(cfg).CreateMapper();
        }
    }
}