namespace Treasury.Application;

﻿using Treasury.Application.Financials.Commands;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Treasury.Application.FinancialTypes.Command;


public partial class MappingProfile : Profile
{

    public void FinancialTypeMappingProfile()
    {
        CreateMap<FinancialType, FinancialTypeDto>();
        CreateMap<FinancialTypeDto, FinancialType>();
        CreateMap<FinancialType, CreateFinancialTypeCommand>();
        CreateMap<CreateFinancialTypeCommand, FinancialType>();

        CreateMap<FinancialType, UpdateFinancialTypeCommand>();
        CreateMap<UpdateFinancialTypeCommand, FinancialType>();
        CreateMap<FinancialType, DeleteFinancialTypeCommand>();
        CreateMap<DeleteFinancialTypeCommand, FinancialType>();

    }
}

