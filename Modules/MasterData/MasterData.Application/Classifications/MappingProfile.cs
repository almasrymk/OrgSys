namespace MasterData.Application;

﻿using MasterData.Application.Classifications.Commands;
using AutoMapper;

public partial class MappingProfile : Profile
{
    public void ClassificationMappingProfile()
    {
        #region Classification
        CreateMap<Classification, ClassificationDto>();
        CreateMap<ClassificationDto, Classification>();


        CreateMap<Classification, CreateClassificationCommand>();
        CreateMap<CreateClassificationCommand, Classification>();
        CreateMap<Classification, UpdateClassificationCommand>();
        CreateMap<UpdateClassificationCommand, Classification>();
        CreateMap<Classification, DeleteClassificationCommand>();
        CreateMap<DeleteClassificationCommand, Classification>();

        CreateMap<ClassificationDto, CreateClassificationCommand>();
        CreateMap<CreateClassificationCommand, ClassificationDto>();
        CreateMap<ClassificationDto, UpdateClassificationCommand>();
        CreateMap<UpdateClassificationCommand, ClassificationDto>();
        #endregion
    }
}
