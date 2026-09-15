namespace Catalog.Application;

﻿using Catalog.Application.Brands.Commands;
using AutoMapper;

public partial class MappingProfile : Profile
{
    public void BrandMappingProfile()
    {
        #region Brand
        CreateMap<Brand, BrandDto>();
        CreateMap<BrandDto, Brand>();

        CreateMap<Brand, CreateBrandCommand>();
        CreateMap<CreateBrandCommand, Brand>();
        CreateMap<Brand, UpdateBrandCommand>();
        CreateMap<UpdateBrandCommand, Brand>();
        CreateMap<Brand, DeleteBrandCommand>();
        CreateMap<DeleteBrandCommand, Brand>();

        CreateMap<BrandDto, CreateBrandCommand>();
        CreateMap<CreateBrandCommand, BrandDto>();
        CreateMap<BrandDto, UpdateBrandCommand>();
        CreateMap<UpdateBrandCommand, BrandDto>();
        #endregion
    }
}
