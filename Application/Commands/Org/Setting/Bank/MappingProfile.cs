using AutoMapper;
using Entity.Model;
using Entity.ModelView;

public partial class MappingProfile : Profile
{
    public void BankMappingProfile()
    {
        #region Bank
        CreateMap<Bank, BankModelView>();
        CreateMap<BankModelView, Bank>();       
        #endregion
    }
}