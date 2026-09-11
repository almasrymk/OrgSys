namespace MasterData.Application;

﻿using AutoMapper;
using MasterData.Application.PaymentTypes.Commands;

public partial class MappingProfile : Profile
{
    public void PaymentTypeMappingProfile()
    {
        #region PaymentType
        CreateMap<PaymentType, PaymentTypeDto>();
        CreateMap<PaymentTypeDto, PaymentType>();       
        #endregion
    }
} 
