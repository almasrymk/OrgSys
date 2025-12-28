using AutoMapper;
using Entity.Model;
using Entity.ModelView;
using Application.Commands.Org.Setting.BankBranch.Commands;

public partial class MappingProfile : Profile
{
    public void BankBranchMappingProfile()
    {
        #region BankBranch
        CreateMap<BankBranch, BankBranchModelView>();
        CreateMap<BankBranchModelView, BankBranch>();       
        #endregion
    }
}