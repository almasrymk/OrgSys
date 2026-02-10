using Application.Commands.Org.Setting.Bank.Commands;
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

        CreateMap<Bank, CreateBankCommand>();
        CreateMap<CreateBankCommand, Bank>();
        CreateMap<Bank, UpdateBankCommand>();
        CreateMap<UpdateBankCommand, Bank>();
        CreateMap<Bank, DeleteBankCommand>();
        CreateMap<DeleteBankCommand, Bank>();

        CreateMap<BankModelView, CreateBankCommand>();
        CreateMap<CreateBankCommand, BankModelView>();
        CreateMap<BankModelView, UpdateBankCommand>();
        CreateMap<UpdateBankCommand, BankModelView>();
        #endregion
    }
}