using Application.Commands.Org.Invoices.Invoice.Commands;
using AutoMapper;
using Entity.Model;
using Entity.ModelView;
using System;
using System.Collections.Generic;
using System.Text;


public partial class MappingProfile : Profile
{

    public void FinancialMappingProfile()
    {
        CreateMap<Entity.Model.Invoice, Entity.Model.Financial>().ReverseMap();
        CreateMap<Entity.Model.Invoice, Entity.Model.FinancialInvoice>().ReverseMap();

        CreateMap<InvoiceModelView, CreateInvoiceCommand>();
        CreateMap<CreateInvoiceCommand, InvoiceModelView>();

        CreateMap<InvoiceModelView, UpdateInvoiceCommand>();
        CreateMap<UpdateInvoiceCommand, InvoiceModelView>();

        CreateMap<Invoice, InvoiceModelView>()
            .ForMember(dest => dest.InvoiceProductList, opt => opt.MapFrom(src => src.InvoiceProducts));
    }
}

