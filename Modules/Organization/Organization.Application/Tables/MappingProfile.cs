namespace Organization.Application;

﻿using AutoMapper;
using Organization.Application.Tables.Commands;

public partial class MappingProfile : Profile
{
    public void TableMappingProfile()
    {
        #region Table
        CreateMap<Table, TableDto>();
        CreateMap<TableDto, Table>();       
        #endregion
    }
}