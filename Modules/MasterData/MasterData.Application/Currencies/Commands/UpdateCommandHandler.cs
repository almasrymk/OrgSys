namespace MasterData.Application.Currencies.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class UpdateCurrencyCommand : CurrencyDto , ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<MasterData.Domain.Currency> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateCurrencyCommand, MasterData.Domain.Currency>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}
