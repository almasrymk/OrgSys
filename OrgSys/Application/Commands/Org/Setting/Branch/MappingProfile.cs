using Application.Commands.Org.Setting.Branch.Commands;
using AutoMapper;
using Entity.Model;
using Entity.ModelView;

public partial class MappingProfile : Profile
{
    public void BranchMappingProfile()
    {
        #region Branch
        CreateMap<Branch, BranchModelView>();
        CreateMap<BranchModelView, Branch>();

        CreateMap<Branch, CreateBranchCommand>();
        CreateMap<CreateBranchCommand, Branch>();
        CreateMap<Branch, UpdateBranchCommand>();
        CreateMap<UpdateBranchCommand, Branch>();
        CreateMap<Branch, DeleteBranchCommand>();
        CreateMap<DeleteBranchCommand, Branch>();

        CreateMap<BranchModelView, CreateBranchCommand>();
        CreateMap<CreateBranchCommand, BranchModelView>();
        CreateMap<BranchModelView, UpdateBranchCommand>();
        CreateMap<UpdateBranchCommand, BranchModelView>();
        #endregion
    }
}