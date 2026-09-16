namespace SaaS.Application;

using SaaS.Application.Features.Commands;
using AutoMapper;

public partial class MappingProfile
{
    public void FeatureMappingProfile()
    {
        #region Feature
        CreateMap<Feature, FeatureDto>();
        CreateMap<FeatureDto, Feature>();

        CreateMap<Feature, CreateFeatureCommand>();
        CreateMap<CreateFeatureCommand, Feature>();
        CreateMap<Feature, UpdateFeatureCommand>();
        CreateMap<UpdateFeatureCommand, Feature>();

        CreateMap<FeatureDto, CreateFeatureCommand>();
        CreateMap<CreateFeatureCommand, FeatureDto>();
        CreateMap<FeatureDto, UpdateFeatureCommand>();
        CreateMap<UpdateFeatureCommand, FeatureDto>();
        #endregion
    }
}
