namespace Treasury.Application.CashBoxes.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class UpdateCashBoxCommand : CashBoxDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Treasury.Domain.CashBox> _Repository, IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateCashBoxCommand, Treasury.Domain.CashBox>(_UnitOfWork, _Repository, mapper, _provider)
    {

    }
}
