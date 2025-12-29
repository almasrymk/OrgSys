using AutoMapper;
using Entity.Model;
using Entity.ModelView;
using Application.Commands.Org.Setting.Account.Commands;

public partial class MappingProfile : Profile
{
    public void AccountMappingProfile()
    {
        #region Account
        CreateMap<Account, AccountModelView>()
        .ForMember(dest => dest.AccountTypeName, opt => opt.MapFrom(src => src.AccountType.Name));

        CreateMap<AccountModelView, Account>();       
        #endregion
    }
}