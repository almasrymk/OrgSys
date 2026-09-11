namespace Treasury.Application.CashBoxes.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class CreateCashBoxCommand : CashBoxDto, ICommand, ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Treasury.Domain.CashBox> _Repository, IMapper mapper) : CreateCommandHandler<CreateCashBoxCommand, Treasury.Domain.CashBox>(_UnitOfWork, _Repository, mapper)
    {

    }
}
