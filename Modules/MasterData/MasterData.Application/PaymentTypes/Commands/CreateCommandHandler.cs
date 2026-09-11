namespace MasterData.Application.PaymentTypes.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed record CreatePaymentTypeCommand(string Name) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<MasterData.Domain.PaymentType> _Repository , IMapper mapper) : CreateCommandHandler<CreatePaymentTypeCommand, MasterData.Domain.PaymentType>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}
