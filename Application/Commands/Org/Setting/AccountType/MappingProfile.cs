using AutoMapper;
using Entity.Model;
using Entity.ModelView;
using Application.Commands.Org.Setting.AccountType.Commands;

public partial class MappingProfile : Profile
{
    public void AccountTypeMappingProfile()
    {
        #region AccountType
        CreateMap<AccountType, AccountTypeModelView>();
        CreateMap<AccountTypeModelView, AccountType>();       
        #endregion
    }
}