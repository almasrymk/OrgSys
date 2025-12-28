using AutoMapper;
using Entity.Model;
using Entity.ModelView;
using Application.Commands.Org.Setting.Account.Commands;

public partial class MappingProfile : Profile
{
    public void AccountMappingProfile()
    {
        #region Account
        CreateMap<Account, AccountModelView>();
        CreateMap<AccountModelView, Account>();       
        #endregion
    }
}