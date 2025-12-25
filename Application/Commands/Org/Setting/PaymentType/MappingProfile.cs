using AutoMapper;
using Entity.Model;
using Entity.ModelView;
using Application.Commands.Org.Setting.PaymentType.Commands;

public partial class MappingProfile : Profile
{
    public void PaymentTypeMappingProfile()
    {
        #region PaymentType
        CreateMap<PaymentType, PaymentTypeModelView>();
        CreateMap<PaymentTypeModelView, PaymentType>();       
        #endregion
    }
}