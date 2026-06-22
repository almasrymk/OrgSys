using AutoMapper;
using Domain.Entities;
using Application.DTOs;
using Application.Commands.Org.Setting.Table.Commands;

public partial class MappingProfile : Profile
{
    public void TableMappingProfile()
    {
        #region Table
        CreateMap<Table, TableModelView>();
        CreateMap<TableModelView, Table>();       
        #endregion
    }
}