using AutoMapper;
using Entity.Model;
using Entity.ModelView;
using Application.Commands.Org.Setting.Dealer.Commands;

public partial class MappingProfile : Profile
{
    public void DealerMappingProfile()
    {
        #region Dealer
        CreateMap<Dealer, DealerModelView>();
        CreateMap<DealerModelView, Dealer>();       
        #endregion
    }
}