using Application.Commands.Org.Setting.BankBranch.Commands;
using AutoMapper;
using Entity.Model;
using Entity.ModelView;

public partial class MappingProfile : Profile
{
    public void BankBranchMappingProfile()
    {
        #region BankBranch
        CreateMap<BankBranch, BankBranchModelView>();
        CreateMap<BankBranchModelView, BankBranch>();

        CreateMap<BankBranch, CreateBankBranchCommand>();
        CreateMap<CreateBankBranchCommand, BankBranch>();
        CreateMap<BankBranch, UpdateBankBranchCommand>();
        CreateMap<UpdateBankBranchCommand, BankBranch>();
        CreateMap<BankBranch, DeleteBankBranchCommand>();
        CreateMap<DeleteBankBranchCommand, BankBranch>();

        CreateMap<BankBranchModelView, CreateBankBranchCommand>();
        CreateMap<CreateBankBranchCommand, BankBranchModelView>();
        CreateMap<BankBranchModelView, UpdateBankBranchCommand>();
        CreateMap<UpdateBankBranchCommand, BankBranchModelView>();
        #endregion
    }
}