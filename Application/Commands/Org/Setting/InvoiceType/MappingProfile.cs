using AutoMapper;
using Domain.Entities;
using Application.DTOs;
using Application.Commands.Org.Setting.InvoiceType.Commands;

public partial class MappingProfile : Profile
{
    public void InvoiceTypeMappingProfile()
    {
        #region InvoiceType
        CreateMap<InvoiceType, InvoiceTypeModelView>();
        CreateMap<InvoiceTypeModelView, InvoiceType>();       
        #endregion
    }
}