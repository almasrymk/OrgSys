namespace Organization.Application;

﻿using Organization.Application.Branches.Commands;
using AutoMapper;

public partial class MappingProfile : Profile
{
    public void BranchMappingProfile()
    {
        #region Branch
        CreateMap<Branch, BranchDto>();
        CreateMap<BranchDto, Branch>();

        CreateMap<Branch, CreateBranchCommand>();
        CreateMap<CreateBranchCommand, Branch>();
        CreateMap<Branch, UpdateBranchCommand>();
        CreateMap<UpdateBranchCommand, Branch>();
        CreateMap<Branch, DeleteBranchCommand>();
        CreateMap<DeleteBranchCommand, Branch>();

        CreateMap<BranchDto, CreateBranchCommand>();
        CreateMap<CreateBranchCommand, BranchDto>();
        CreateMap<BranchDto, UpdateBranchCommand>();
        CreateMap<UpdateBranchCommand, BranchDto>();
        #endregion
    }
}