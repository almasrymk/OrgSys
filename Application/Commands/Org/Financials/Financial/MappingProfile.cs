using Application.Commands.Org.Financials.Financial.Commands;
using Application.Commands.Org.Invoices.Invoice.Commands;
using AutoMapper;
using Domain.Entities;
using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;


public partial class MappingProfile : Profile
{

    public void FinancialMappingProfile()
    {
        CreateMap<Financial, FinancialModelView>()
            .ForMember(dest => dest.FinancialInvoiceList, opt => opt.MapFrom(src => src.FinancialInvoices))
            ;
        CreateMap<FinancialModelView, Financial>();

        CreateMap<Financial, CreateFinancialCommand>();
        CreateMap<CreateFinancialCommand, Financial>();

        CreateMap<Financial, UpdateFinancialCommand>();
        CreateMap<UpdateFinancialCommand, Financial>();

        CreateMap<Financial, DeleteFinancialCommand>();
        CreateMap<DeleteFinancialCommand, Financial>();

        CreateMap<Invoice, Financial>();
        CreateMap<Financial, Invoice>();

        CreateMap<FinancialInvoice, FinancialInvoiceModelView>()
            .ForMember(dest => dest.Net , src => src.MapFrom(s => s.Invoice.Net));
        CreateMap<FinancialInvoiceModelView, FinancialInvoice>();


        CreateMap<CreateFinancialCommand, Financial>().ForMember(dest => dest.FinancialInvoices,            
opt => opt.MapFrom(src => src.FinancialInvoices));  


        CreateMap<UpdateFinancialCommand, Financial>()
.ForMember(dest => dest.FinancialInvoices,
opt => opt.MapFrom(src => src.FinancialInvoices));

    }
}

