namespace MasterData.Application.PaymentTypes.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed record UpdatePaymentTypeCommand(long Id , long? PaymentTypeId, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<MasterData.Domain.PaymentType> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdatePaymentTypeCommand, MasterData.Domain.PaymentType>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}
