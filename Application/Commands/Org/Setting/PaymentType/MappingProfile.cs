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

        CreateMap<PaymentType, CreatePaymentTypeCommand>();
        CreateMap<CreatePaymentTypeCommand, PaymentType>();
        CreateMap<PaymentType, UpdatePaymentTypeCommand>();
        CreateMap<UpdatePaymentTypeCommand, PaymentType>();
        CreateMap<PaymentType, DeletePaymentTypeCommand>();
        CreateMap<DeletePaymentTypeCommand, PaymentType>();

        CreateMap<PaymentTypeModelView, CreatePaymentTypeCommand>();
        CreateMap<CreatePaymentTypeCommand, PaymentTypeModelView>();
        CreateMap<PaymentTypeModelView, UpdatePaymentTypeCommand>();
        CreateMap<UpdatePaymentTypeCommand, PaymentTypeModelView>();   
        #endregion
    }
}