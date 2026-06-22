using Application.Commands.Org.Financials.Financial.Commands;
using Application.Commands.Org.Financials.FinancialType.Commands;
using Application.Commands.Org.Invoices.Invoice.Commands;
using Application.Commands.Org.Transactions.TransactionType.Commands;
using AutoMapper;
using Domain.Entities;
using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;


public partial class MappingProfile : Profile
{

    public void FinancialTypeMappingProfile()
    {
        CreateMap<FinancialType, FinancialTypeModelView>();
        CreateMap<FinancialTypeModelView, FinancialType>();
        CreateMap<FinancialType, CreateFinancialTypeCommand>();
        CreateMap<CreateFinancialTypeCommand, FinancialType>();

        CreateMap<FinancialType, UpdateFinancialTypeCommand>();
        CreateMap<UpdateFinancialTypeCommand, FinancialType>();
        CreateMap<FinancialType, DeleteFinancialTypeCommand>();
        CreateMap<DeleteFinancialTypeCommand, FinancialType>();

    }
}

