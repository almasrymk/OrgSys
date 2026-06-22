using Application.Commands.Org.Setting.Classification.Commands;
using AutoMapper;
using Domain.Entities;
using Application.DTOs;

public partial class MappingProfile : Profile
{
    public void ClassificationMappingProfile()
    {
        #region Classification
        CreateMap<Classification, ClassificationModelView>();
        CreateMap<ClassificationModelView, Classification>();


        CreateMap<Classification, CreateClassificationCommand>();
        CreateMap<CreateClassificationCommand, Classification>();
        CreateMap<Classification, UpdateClassificationCommand>();
        CreateMap<UpdateClassificationCommand, Classification>();
        CreateMap<Classification, DeleteClassificationCommand>();
        CreateMap<DeleteClassificationCommand, Classification>();

        CreateMap<ClassificationModelView, CreateClassificationCommand>();
        CreateMap<CreateClassificationCommand, ClassificationModelView>();
        CreateMap<ClassificationModelView, UpdateClassificationCommand>();
        CreateMap<UpdateClassificationCommand, ClassificationModelView>();
        #endregion
    }
}