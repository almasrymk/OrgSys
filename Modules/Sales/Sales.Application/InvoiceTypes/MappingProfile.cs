namespace Sales.Application;

﻿using AutoMapper;
using Sales.Application.InvoiceTypes.Commands;

public partial class MappingProfile : Profile
{
    public void InvoiceTypeMappingProfile()
    {
        #region InvoiceType
        CreateMap<InvoiceType, InvoiceTypeDto>();
        CreateMap<InvoiceTypeDto, InvoiceType>();       
        #endregion
    }
}