namespace Treasury.Application;

﻿using Treasury.Application.Financials.Commands;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;


public partial class MappingProfile : Profile
{

    public void FinancialMappingProfile()
    {
        CreateMap<Financial, FinancialDto>()
            .ForMember(dest => dest.FinancialInvoiceList, opt => opt.MapFrom(src => src.FinancialInvoices))
            ;
        CreateMap<FinancialDto, Financial>();

        CreateMap<Financial, CreateFinancialCommand>();
        CreateMap<CreateFinancialCommand, Financial>();
        // The MVC-side generic Save(TDto) flow (MainController<>.Save, reused by the Opening Balance
        // Draft path) maps the posted FinancialDto straight into CreateFinancialCommand/UpdateFinancialCommand
        // before dispatching — without these, that mapping throws at runtime the first time it runs.
        CreateMap<FinancialDto, CreateFinancialCommand>();
        CreateMap<FinancialDto, UpdateFinancialCommand>();

        CreateMap<Financial, UpdateFinancialCommand>();
        CreateMap<UpdateFinancialCommand, Financial>();

        CreateMap<Financial, DeleteFinancialCommand>();
        CreateMap<DeleteFinancialCommand, Financial>();

        CreateMap<FinancialInvoice, FinancialInvoiceDto>()
            .ForMember(dest => dest.Net, opt => opt.Ignore());
        CreateMap<FinancialInvoiceDto, FinancialInvoice>();


        CreateMap<CreateFinancialCommand, Financial>().ForMember(dest => dest.FinancialInvoices,            
opt => opt.MapFrom(src => src.FinancialInvoices));  


        CreateMap<UpdateFinancialCommand, Financial>()
.ForMember(dest => dest.FinancialInvoices,
opt => opt.MapFrom(src => src.FinancialInvoices));

    }
}

