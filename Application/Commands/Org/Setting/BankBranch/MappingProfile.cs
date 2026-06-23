using Application.Commands.Org.Setting.BankBranch.Commands;
using AutoMapper;
using Domain.Entities;
using Application.DTOs;

public partial class MappingProfile : Profile
{
    public void BankBranchMappingProfile()
    {
        #region BankBranch
        CreateMap<BankBranch, BankBranchDto>();
        CreateMap<BankBranchDto, BankBranch>();

        CreateMap<BankBranch, CreateBankBranchCommand>();
        CreateMap<CreateBankBranchCommand, BankBranch>();
        CreateMap<BankBranch, UpdateBankBranchCommand>();
        CreateMap<UpdateBankBranchCommand, BankBranch>();
        CreateMap<BankBranch, DeleteBankBranchCommand>();
        CreateMap<DeleteBankBranchCommand, BankBranch>();

        CreateMap<BankBranchDto, CreateBankBranchCommand>();
        CreateMap<CreateBankBranchCommand, BankBranchDto>();
        CreateMap<BankBranchDto, UpdateBankBranchCommand>();
        CreateMap<UpdateBankBranchCommand, BankBranchDto>();
        #endregion
    }
}