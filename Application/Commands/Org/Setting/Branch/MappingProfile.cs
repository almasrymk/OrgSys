using AutoMapper;
using Entity.Model;
using Entity.ModelView;
using Application.Commands.Org.Setting.Branch.Commands;

public partial class MappingProfile : Profile
{
    public void BranchMappingProfile()
    {
        #region Branch
        CreateMap<Branch, BranchModelView>();
        CreateMap<BranchModelView, Branch>();       
        #endregion
    }
}