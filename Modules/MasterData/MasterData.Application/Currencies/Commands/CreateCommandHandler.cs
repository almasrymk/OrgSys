namespace MasterData.Application.Currencies.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class CreateCurrencyCommand : CurrencyDto , ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<MasterData.Domain.Currency> _Repository , IMapper mapper) : CreateCommandHandler<CreateCurrencyCommand, MasterData.Domain.Currency>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}
